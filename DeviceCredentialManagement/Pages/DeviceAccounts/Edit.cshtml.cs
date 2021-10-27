using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceCredentialManagement.Model;
using DeviceCredentialManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DeviceCredentialManagement.Pages.DeviceAccounts
{
    [Authorize(Roles = "administrator,user")]
    public class EditModel : PageModel
    {
        private readonly ILogger _logger;
        private IDeviceAccountRepository deviceAccountRepository;
        [BindProperty]
        public DeviceAccountCredential DeviceAccount { get; set; }

        public string GeneratedPassword { get; set; }

        public string Message { get; set; }

        public string ErrorMessage { get; set; }
        public EditModel(IDeviceAccountRepository deviceAccountRepository, ILogger<EditModel> logger)
        {
            this.deviceAccountRepository = deviceAccountRepository;
            this._logger = logger;
        }

        public IActionResult OnGet(int? id)
        {
            if (id.HasValue)
            {
                _logger.LogInformation("Edit page visited for account id: "+id);
                DeviceAccount = deviceAccountRepository.GetAccountCredential(id.Value);
                GeneratedPassword = DeviceAccount.Password;
            }
            else
            {
                _logger.LogInformation("Add page visited");
                DeviceAccount = new DeviceAccountCredential();
            }
            if (DeviceAccount == null)
            {
                return RedirectToPage("/NotFound");
            }

            return Page();
        }

       

        public IActionResult OnPostGeneratePassword()
        {
            GeneratedPassword = deviceAccountRepository.GeneratePassword(DeviceAccount.AccountName,DeviceAccount.AccountFor,DeviceAccount.AccountReferenceNumber);
            _logger.LogInformation("Password generated for Accountid: " + DeviceAccount.Password + ", AccountName: " + DeviceAccount.AccountName
                +  "Type : " +DeviceAccount.AccountFor+ " Reference number: "+ DeviceAccount.AccountReferenceNumber);
            return Page();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(DeviceAccount.Password))
                {
                    ErrorMessage = "Please generate password or enter password to save details";
                }
                else
                {
                    GeneratedPassword = DeviceAccount.Password;
                    if (DeviceAccount.Id > 0)
                    {
                        DeviceAccountCredential updateddeviceAccount = deviceAccountRepository.UpdateDeviceAccount(DeviceAccount);
                        if (updateddeviceAccount != null)
                        {
                            DeviceAccount = updateddeviceAccount;
                            Message = "Your details are updated for the account. Go to ";
                            _logger.LogInformation("Account is updated for Accountid: "+ DeviceAccount.Id+ "AccountName: " + DeviceAccount.AccountName + "Type : "
                                + DeviceAccount.AccountFor + " Reference number: " + DeviceAccount.AccountReferenceNumber);
                        }
                        else
                        {
                            ErrorMessage = "Your details could not be updated as Account Name and Reference number already exists ";
                            _logger.LogWarning("Account could not be updated for id: " + DeviceAccount.Id);
                        }
                        
                        //return RedirectToPage("Index");
                    }
                    else
                    {
                        DeviceAccountCredential addeddeviceAccount =  deviceAccountRepository.AddDeviceAccount(DeviceAccount);
                        if (addeddeviceAccount != null)
                        {
                            DeviceAccount = addeddeviceAccount;
                            Message = "Your details are added for the account. Go to ";
                            _logger.LogInformation("Account is added successfully for Accountid: " + DeviceAccount.Id + "AccountName: " + DeviceAccount.AccountName + "Type : "
                              + DeviceAccount.AccountFor + " Reference number: " + DeviceAccount.AccountReferenceNumber);
                        }
                        else
                        {
                            ErrorMessage = "Your details could not be added ";
                            _logger.LogWarning("Account could not be added for id: " + DeviceAccount.Id);
                        }
                    }
                }
            }
            return Page();
        }
    }
}
