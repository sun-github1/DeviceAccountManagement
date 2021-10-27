using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceCredentialManagement.Model;
using DeviceCredentialManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CandidCredentialManagement.Pages.DeviceAccounts
{
    [Authorize(Roles = "administrator,user")]
    public class BatchModel : PageModel
    {
        private readonly ILogger _logger;
        private IDeviceAccountRepository deviceAccountRepository;
        [BindProperty]
        public DeviceAccountsBatch DeviceAccounts { get; set; }

        [BindProperty]
        public string GeneratedPasswords { get; set; }

        public string Message { get; set; }

        public string ErrorMessage { get; set; }

        public BatchModel(IDeviceAccountRepository deviceAccountRepository, ILogger<BatchModel> logger)
        {
            this.deviceAccountRepository = deviceAccountRepository;
            this._logger = logger;
        }

        public IActionResult OnGet()
        {

            _logger.LogInformation("AddInBatch page visited  ");
            DeviceAccounts = new DeviceAccountsBatch();

            return Page();
        }

        public IActionResult OnPostGeneratePasswords()
        {
            GeneratedPasswords = "";
            string[] referenceArrays = null; //yourString.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                             //  GeneratedPasswords = candidAccountRepository.GeneratePassword(CandidAccount.AccountName, CandidAccount.AccountFor, CandidAccount.AccountReferenceNumber);
            StringBuilder allpasswords = new StringBuilder();
            if (DeviceAccounts.AccountReferenceNumbers.Contains(','))
            {
                referenceArrays = DeviceAccounts.AccountReferenceNumbers.Split(',');
            }
            else
            {
                referenceArrays = DeviceAccounts.AccountReferenceNumbers.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);// DeviceAccounts.AccountReferenceNumbers.Split(',');
            }

            foreach (string eachReference in referenceArrays)
            {
                allpasswords.Append(deviceAccountRepository.GeneratePassword(DeviceAccounts.AccountName, DeviceAccounts.AccountFor, eachReference));
                allpasswords.Append('|');
            }

            GeneratedPasswords = allpasswords.ToString();
            DeviceAccounts.Passwords = GeneratedPasswords;
            ModelState["GeneratedPasswords"].RawValue = allpasswords.ToString();
            //ModelState["DeviceAccounts.Passwords"].RawValue = GeneratedPasswords;
            _logger.LogInformation("Password generated for Accountid: " + DeviceAccounts.Passwords + ", AccountName: " + DeviceAccounts.AccountName
                + "Type : " + DeviceAccounts.AccountFor + " Reference numbers: " + DeviceAccounts.AccountReferenceNumbers);
            return Page();
        }


        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                DeviceAccounts.Passwords = GeneratedPasswords;
                if (string.IsNullOrEmpty(DeviceAccounts.Passwords))
                {
                    ErrorMessage = "Please generate password or enter password to save details";
                }
                else
                {
                 

                    bool allsuccess = true;
                    // string message = "All your details added successfully. Go to ";
                    //if (DeviceAccounts.Id > 0)
                    //{
                    string[] generatedPasswordArr = DeviceAccounts.Passwords.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] generatedReferenceArr = DeviceAccounts.AccountReferenceNumbers.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < generatedPasswordArr.Count(); i++)
                    {
                        DeviceAccountCredential candidAccount = new DeviceAccountCredential()
                        {
                            // Id=DeviceAccounts.Id,
                            AccountFor = DeviceAccounts.AccountFor,
                            AccountName = DeviceAccounts.AccountName,
                            AccountReferenceNumber = generatedReferenceArr[i],
                            Username = DeviceAccounts.Username,
                            Password = generatedPasswordArr[i],

                        };
                        DeviceAccountCredential addedcandidAccount = deviceAccountRepository.AddDeviceAccount(candidAccount);
                        if (addedcandidAccount != null)
                        {
                            //CandidAccount = updatedcandidAccount;
                            // Message = "Your details are updated for the account. Go to ";
                            _logger.LogInformation("Account is added for Accountid: " + addedcandidAccount.Id + "AccountName: " + addedcandidAccount.AccountName + "Type : "
                                + addedcandidAccount.AccountFor + " Reference number: " + addedcandidAccount.AccountReferenceNumber);
                        }
                        else
                        {
                            allsuccess = false;
                            // ErrorMessage = "Your details could not be updated as Account Name and Reference number already exists ";
                            _logger.LogWarning($"Account could not be added for reference number {candidAccount.AccountReferenceNumber}");
                        }

                    }

                    if (allsuccess)
                    {
                        Message = "All your details are updated for the account. Go to ";
                    }
                    else
                    {
                        ErrorMessage = "One or more of your details could not be updated as Account Name and Reference number already exists ";
                    }

                    //return RedirectToPage("Index");
                    //}
                    //else
                    //{
                    //    CandidAccountCredential addeccandidAccount = candidAccountRepository.AddCandidAccount(CandidAccount);
                    //    if (addeccandidAccount != null)
                    //    {
                    //        CandidAccount = addeccandidAccount;
                    //        Message = "Your details are added for the account. Go to ";
                    //        _logger.LogInformation("Account is added successfully for Accountid: " + CandidAccount.Id + "AccountName: " + CandidAccount.AccountName + "Type : "
                    //          + CandidAccount.AccountFor + " Reference number: " + CandidAccount.AccountReferenceNumber);
                    //    }
                    //    else
                    //    {
                    //        ErrorMessage = "Your details could not be added ";
                    //        _logger.LogWarning("Account could not be added for id: " + CandidAccount.Id);
                    //    }
                    //}
                }
            }
            return Page();
        }
    }
}
