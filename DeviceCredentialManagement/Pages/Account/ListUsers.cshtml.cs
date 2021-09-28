using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DeviceCredentialManagement.Pages.Account
{

    [Authorize(Roles = "administrator")]
    public class ListUsersModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly UserManager<IdentityUser> userManager;


        [BindProperty]
        public List<IdentityUser> Users { get; set; }
        public ListUsersModel(ILogger<RegisterModel> _logger,
            UserManager<IdentityUser> userManager)
        {
            this._logger = _logger;
            this.userManager = userManager;
        }


        public void OnGet()
        {
            if (userManager.Users != null)
            {
                Users = userManager.Users.ToList();
            }
        }

        public async Task<IActionResult> OnPostDeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                //ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                TempData["message"] = $"User with Id = {id} cannot be found";
                return RedirectToPage("/NotFound");
            }
            else
            {

                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToPage("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return Page();
            }
        }
    }
}
