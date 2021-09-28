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
    public class ManageUserRolesModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        [BindProperty]
        public List<UsersRole> ListUserRoles { get; set; }
        [BindProperty]
        public string UserId { get; set; }
        [BindProperty]
        public string UserName { get; set; }

        public ManageUserRolesModel(ILogger<EditRoleModel> _logger,
            RoleManager<IdentityRole> roleManager,
             UserManager<IdentityUser> userManager)
        {
            this._logger = _logger;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public async Task<IActionResult> OnGet(string userid)
        {
            UserId = userid;

            var user = await userManager.FindByIdAsync(userid);

            if (user == null)
            {
                TempData["message"] = $"User with Id = {userid} cannot be found";
                return RedirectToPage("/NotFound");
            }
            UserName = user.UserName;
            ListUserRoles = new List<UsersRole>();

            foreach (var role in roleManager.Roles.ToList())
            {
                var eachUserRole = new UsersRole
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    eachUserRole.IsSelected = true;
                }
                else
                {
                    eachUserRole.IsSelected = false;
                }

                ListUserRoles.Add(eachUserRole);
            }
            return Page();
        }


        public async Task<IActionResult> OnPost()
        {
            var user = await userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                TempData["message"] = $"User with Id = {UserId} cannot be found";
                return RedirectToPage("/NotFound");
            }
           
            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return Page();
            }

            result = await userManager.AddToRolesAsync(user,
                ListUserRoles.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return Page();
            }

            return RedirectToPage("EditUser", new { id = UserId });
        }
    }
}
