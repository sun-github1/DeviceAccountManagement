using DeviceCredentialManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceCredentialManagement.Services
{
    public class MockDeviceAccountRepository : IDeviceAccountRepository
    {
        public List<DeviceAccountCredential> _candidAccountList;
      
        public MockDeviceAccountRepository()
        {
            _candidAccountList = new List<DeviceAccountCredential>()
            {
                new DeviceAccountCredential() { Id = 1, AccountName = "ENOC1020", AccountReferenceNumber="123456879",
                    AccountFor =AccountType.DPU,Username= "ANPR", Password="anpr@123"},
                new DeviceAccountCredential() { Id = 2, AccountName = "ENOC1020", AccountReferenceNumber="123456879",
                    AccountFor =AccountType.Camera,Username= "admin", Password="candid" },
                new DeviceAccountCredential() { Id = 3, AccountName = "ENOC1060", AccountReferenceNumber="123456810",
                    AccountFor =AccountType.DPU,Username= "administrator", Password="this#isalong$password@123" },
            };
        }

        public IEnumerable<DeviceAccountCredential> GetAllAccountCredentials()
        {
            return _candidAccountList;
        }

        public DeviceAccountCredential GetAccountCredential(int accountId)
        {
            return _candidAccountList.FirstOrDefault(x=>x.Id== accountId);
        }

        public DeviceAccountCredential UpdateDeviceAccount(DeviceAccountCredential updatedCandidAccount)
        {
            DeviceAccountCredential candidAccount = _candidAccountList.FirstOrDefault(e => e.Id == updatedCandidAccount.Id);
            if (candidAccount != null)
            {
                candidAccount.AccountName = updatedCandidAccount.AccountName;
                candidAccount.AccountFor = updatedCandidAccount.AccountFor;
                candidAccount.AccountReferenceNumber = updatedCandidAccount.AccountReferenceNumber;
                candidAccount.Password = updatedCandidAccount.Password;
                candidAccount.Username = updatedCandidAccount.Username;
            }
            return candidAccount;
        }

        public DeviceAccountCredential AddDeviceAccount(DeviceAccountCredential newCandidAccount)
        {
            newCandidAccount.Id = _candidAccountList.Max(e => e.Id) + 1;
            _candidAccountList.Add(newCandidAccount);
            return newCandidAccount;
        }

        public DeviceAccountCredential DeleteDeviceAccount(int id)
        {
            DeviceAccountCredential candidAccount = _candidAccountList.FirstOrDefault(e => e.Id == id);
            if (candidAccount != null)
            {
                _candidAccountList.Remove(candidAccount);
            }
            return candidAccount;
        }

        public IEnumerable<DeviceAccountCredential> SearchAccountCredentials(string searchTerm)
        {
            return _candidAccountList.Where(e => e.AccountName.Contains(searchTerm) ||
                                        e.AccountReferenceNumber.Contains(searchTerm) || 
                                        e.Id.ToString()==searchTerm).ToList();
        }

        public string GeneratePassword(string accountName, AccountType? accountFor, string accountReferenceNumber)
        {
            return "156465";
        }

        public IEnumerable<DeviceAccountCredential> GetAccountCredentialByAccountName(string accountName)
        {
            throw new Exception("Method not implemented ");
        }

        public int GetTotalAccountCredentials(string searchTerm)
        {
            throw new Exception("Method not implemented ");
        }

        public IEnumerable<DeviceAccountCredential> GetAccountCredentials(int pageIndex, int pageSize, string searchTerm)
        {
            throw new Exception("Method not implemented ");
        }
    }
}
