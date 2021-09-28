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
    public class DeleteModel : PageModel
    {
        private readonly ILogger _logger;
        private IDeviceAccountRepository deviceAccountRepository;
        [BindProperty]
        public DeviceAccountCredential DeviceAccount { get; set; }

        public DeleteModel(IDeviceAccountRepository deviceAccountRepository, ILogger<DeleteModel> logger)
        {
            this.deviceAccountRepository = deviceAccountRepository;
            this._logger = logger;
        }

        public IActionResult OnPost()
        {
            DeviceAccountCredential deletedAccount = deviceAccountRepository.DeleteDeviceAccount(DeviceAccount.Id);

            if (deletedAccount == null)
            {
                _logger.LogInformation("Account could not be deleted:" + deletedAccount.Id);
                return RedirectToPage("/NotFound");
            }
            else
            {
                _logger.LogInformation("Account has been deleted having AccountId:" + deletedAccount.Id);
            }

            return RedirectToPage("/DeviceAccounts/Index");
        }

        public IActionResult OnGet(int id)
        {
            
            _logger.LogInformation("Delete page visited for Account Id:"+id);
                DeviceAccount = deviceAccountRepository.GetAccountCredential(id);
                if (DeviceAccount != null)
                {
                    return Page();
                }
                else
                {
                    return RedirectToPage("/NotFound");
                }

        }
    }
}
