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
    [Authorize(Roles = "administrator")]
    public class EditUserModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly UserManager<IdentityUser> userManager;

        [BindProperty]
        public EditUser UserToEdit { get; set; }
        [BindProperty]
        public IList<string> Roles { get; set; }

        public EditUserModel(ILogger<EditUserModel> _logger, UserManager<IdentityUser> userManager)
        {
            this._logger = _logger;
            this.userManager = userManager;
        }
        //public async Task<IActionResult> OnGet(string id)
        //{
        //    IdentityUser user = await userManager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        return RedirectToPage("/NotFound");
        //    }

        //    var rolesForUser = await userManager.GetRolesAsync(user);
        //    Roles = rolesForUser;
        //    return Page();
        //}

        public async Task<IActionResult> OnGet(string id)
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
                UserToEdit = new EditUser()
                {
                    Id= user.Id,
                    Email=user.Email,
                    UserName=user.UserName
                };

                var rolesForUser = await userManager.GetRolesAsync(user);
                Roles = rolesForUser;

                //user.Email = UserToEdit.Email;
                //user.UserName = UserToEdit.UserName;

                //var result = await userManager.UpdateAsync(user);

                //if (result.Succeeded)
                //{
                //    return RedirectToPage("ListUsers");
                //}

                //foreach (var error in result.Errors)
                //{
                //    ModelState.AddModelError("", error.Description);
                //}

                return Page();
            }
            
        }


        public async Task<IActionResult> OnPost(string id)
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
                user.Email = UserToEdit.Email;
                user.UserName = UserToEdit.UserName;
                var result = await userManager.UpdateAsync(user);

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
