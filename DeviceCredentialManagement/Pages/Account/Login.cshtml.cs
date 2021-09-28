using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceCredentialManagement.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DeviceCredentialManagement.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly SignInManager<IdentityUser> signInManager;

        [BindProperty]
        public LoginUser LoginUser { get; set; }

        public LoginModel(ILogger<LoginModel> _logger, SignInManager<IdentityUser> signInManager)
        {
            this._logger = _logger;
            this.signInManager = signInManager;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost( string returnUrl)
        {
            if(ModelState.IsValid)
            {
                var result =await signInManager.PasswordSignInAsync(LoginUser.Email, LoginUser.Password, LoginUser.RememberMe, false);
                if(result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) && returnUrl!="/")
                    {
                        return RedirectToPage(returnUrl);
                    }
                    else
                    {
                        return RedirectToPage("/DeviceAccounts/Index");
                    }
                }
                ModelState.AddModelError("","Invalid Login Attempt. Please verify credentials");
            }
            return Page();
        }
    }
}
