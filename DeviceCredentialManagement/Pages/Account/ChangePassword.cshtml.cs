using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceCredentialManagement.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DeviceCredentialManagement.Pages.Account
{
    public class ChangePasswordModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public ChangePasswordModel(ILogger<RegisterModel> _logger,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this._logger = _logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [BindProperty]
        public ChangePassword ChangePassword { get; set; }

        public IActionResult OnGet()
        {
            //var user = await userManager.GetUserAsync(User);
            //if (user == null)
            //{
            //    return RedirectToAction("Login");
            //}
            ChangePassword = new ChangePassword();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                // ChangePasswordAsync changes the user password
                var result = await userManager.ChangePasswordAsync(user,
                    ChangePassword.CurrentPassword, ChangePassword.NewPassword);

                // The new password did not meet the complexity rules or
                // the current password is incorrect. Add these errors to
                // the ModelState and rerender ChangePassword view
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }

                // Upon successfully changing the password refresh sign-in cookie
                await signInManager.RefreshSignInAsync(user);

                await signInManager.SignOutAsync();
                return RedirectToPage("ChangePasswordConfirmation");
            }

            return Page();
        }
    }
}
