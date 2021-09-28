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
    public class EditRoleModel : PageModel
    {

        private readonly ILogger _logger;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        [BindProperty]
        public EditRole RoleEdit { get; set; }

        [BindProperty]
        public List<string> Users { get; set; }


        public EditRoleModel(ILogger<EditRoleModel> _logger,
            RoleManager<IdentityRole> roleManager,
             UserManager<IdentityUser> userManager)
        {
            this._logger = _logger;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGet(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                IdentityRole role = await roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return RedirectToPage("/NotFound");
                }
                RoleEdit = new EditRole();
                RoleEdit.Id = role.Id;
                RoleEdit.RoleName = role.Name;
                Users = new List<string>();
                foreach (var eachUser in userManager.Users.ToList())
                {
                    if (await userManager.IsInRoleAsync(eachUser, role.Name))
                    {
                        Users.Add(eachUser.UserName);
                    }
                }
                return Page();
            }
            else
            {
                return RedirectToPage("/NotFound");
            }
        }
        public async Task<IActionResult> OnPost()
        {
            if (!string.IsNullOrEmpty(RoleEdit.Id))
            {
                IdentityRole role = await roleManager.FindByIdAsync(RoleEdit.Id);
                if (role == null)
                {
                    ModelState.AddModelError("", "Role with this id not found");
                }
                role.Name = RoleEdit.RoleName;

               var result=await roleManager.UpdateAsync(role);
                if(result.Succeeded)
                {
                    return RedirectToPage("ListRoles");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                return Page();
            }
            else
            {
                return RedirectToPage("/NotFound");
            }
        }
    }
}
