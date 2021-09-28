using DeviceCredentialManagement.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceCredentialManagement.Services.Classes
{
    public class DeviceAccountRepository : IDeviceAccountRepository
    {
        private readonly AppDBContext context;
        const string specialchars = "@#!-";
        private static Random random = new Random();
        private ILogger<DeviceAccountRepository> logger;
        public DeviceAccountRepository(AppDBContext context, ILogger<DeviceAccountRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public DeviceAccountCredential AddDeviceAccount(DeviceAccountCredential newCandidAccount)
        {
            try
            {
                List<DeviceAccountCredential> canndidAcountList = context.CandidAccountCredentials.ToList();

                if (canndidAcountList.FirstOrDefault(x => x.AccountName == newCandidAccount.AccountName
                    && x.AccountReferenceNumber == newCandidAccount.AccountReferenceNumber) == null)
                {
                    newCandidAccount.Id = canndidAcountList != null && canndidAcountList.Count > 0 ? (canndidAcountList.Max(e => e.Id) + 1) : 1;
                    newCandidAccount.CreateUpdateDate = DateTime.Now;
                    newCandidAccount.LastRecordOperation = "I";
                    context.CandidAccountCredentials.Add(newCandidAccount);
                    context.SaveChanges();
                    return newCandidAccount;
                }
                else
                {
                    return null;//alreday exists
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw ex;
            }

        }

        public DeviceAccountCredential DeleteDeviceAccount(int id)
        {
            try
            {
                DeviceAccountCredential candidAccount = context.CandidAccountCredentials.FirstOrDefault(e => e.Id == id);
                if (candidAccount != null)
                {
                    context.CandidAccountCredentials.Remove(candidAccount);
                    context.SaveChanges();
                }
                return candidAccount;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw ex;
            }
        }

        public DeviceAccountCredential GetAccountCredential(int accountId)
        {
            try
            {
                return context.CandidAccountCredentials.FirstOrDefault(x => x.Id == accountId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw ex;
            }
        }

        public IEnumerable<DeviceAccountCredential> GetAllAccountCredentials()
        {
            logger.LogInformation("CandidAccountRepository: GetAllAccountCredentials called");
            //IQueryable<CandidAccountCredential> source = context.CandidAccountCredentials;

            //var count = source.Count();
            //var items = source.Skip(
            //    (pageIndex - 1) * pageSize)
            //    .Take(pageSize).ToList();
            //return items;

            return context.CandidAccountCredentials.OrderBy(x => x.Id);
        }

        public IEnumerable<DeviceAccountCredential> SearchAccountCredentials(string searchTerm)
        {
            return context.CandidAccountCredentials.Where(e => e.AccountName.Contains(searchTerm) ||
                                       e.AccountReferenceNumber.Contains(searchTerm) ||
                                       e.Id.ToString() == searchTerm).OrderBy(x => x.Id).ToList();
        }

        public DeviceAccountCredential UpdateDeviceAccount(DeviceAccountCredential updatedCandidAccount)
        {
            //List<CandidAccountCredential> canndidAcountList = context.CandidAccountCredentials.ToList();

            //if (canndidAcountList.FirstOrDefault(x => x.Id == updatedCandidAccount.Id) != null)
            //{
            // return context.Employees
            //.FromSqlRaw<Employee>("spGetEmployeeById {0}", id)
            //.ToList()
            //.FirstOrDefault();
            try
            {
                updatedCandidAccount.LastRecordOperation = "U";
                updatedCandidAccount.CreateUpdateDate = DateTime.Now;

                var candidAccount = context.CandidAccountCredentials.Attach(updatedCandidAccount);
                candidAccount.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();

                return updatedCandidAccount;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw ex;
            }

        }

        public IEnumerable<DeviceAccountCredential> GetAccountCredentialByAccountName(string accountName)
        {
            return context.CandidAccountCredentials.Where(e => e.AccountName == accountName).OrderBy(x => x.Id);
        }

        public string GeneratePassword(string accountName, AccountType? accountFor, string accountReferenceNumber)
        {
            accountReferenceNumber = accountReferenceNumber.Replace(" ", "").Replace(":", "").Replace("-", "");
            string generatedpassword = (accountFor == AccountType.DPU ? "candid" : "cnd")
                + (accountName.Replace(" ", "").Length > 4 ? accountName.Replace(" ", "").Substring(0, 4) : accountName.Replace(" ", ""))
                + accountFor.ToString().ToCharArray()[0].ToString()//D or C
                + specialchars[random.Next(specialchars.Length)]//random generated special character
                + (accountReferenceNumber.Length > 4 ? accountReferenceNumber.Substring(accountReferenceNumber.Length - 4, 4) : accountReferenceNumber);//last fpur digit of ref no
            return generatedpassword;
        }

        public int GetTotalAccountCredentials(string searchTerm)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                return context.CandidAccountCredentials.Where(e => e.AccountName.Contains(searchTerm) ||
                                         e.AccountReferenceNumber.Contains(searchTerm) ||
                                         e.Id.ToString() == searchTerm).Count();
            }
            else
            {
                return context.CandidAccountCredentials.Count();
            }
        }

        public IEnumerable<DeviceAccountCredential> GetAccountCredentials(int pageIndex, int pageSize, string searchTerm)
        {
            IQueryable<DeviceAccountCredential> query;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query= context.CandidAccountCredentials.Where(e => e.AccountName.Contains(searchTerm) ||
                                         e.AccountReferenceNumber.Contains(searchTerm) ||
                                         e.Id.ToString() == searchTerm);
            }
            else
            {
                query = context.CandidAccountCredentials;
            }

            return query.OrderBy(x=>x.Id).Skip((pageIndex-1) * pageSize).Take(pageSize).ToList();
        }
    }
}
