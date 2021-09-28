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
    public class EditUsersInRoleModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        [BindProperty]
        public List<UserForRole> UserRoleModel { get; set; }
        [BindProperty]
        public string RoleId { get; set; }

        public EditUsersInRoleModel(ILogger<EditRoleModel> _logger,
                    RoleManager<IdentityRole> roleManager,
                     UserManager<IdentityUser> userManager)
        {
            this._logger = _logger;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGet(string roleId)
        {
            RoleId = roleId;
            IdentityRole role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return RedirectToPage("/NotFound");
            }
            UserRoleModel = new List<UserForRole>();
            foreach (var eachUser in userManager.Users.ToList())
            {
                UserForRole userrole = new UserForRole();
                userrole.UserId = eachUser.Id;
                userrole.UserName = eachUser.UserName;
                if (await userManager.IsInRoleAsync(eachUser, role.Name))
                {
                    userrole.IsSelected = true;
                }
                else
                {
                    userrole.IsSelected = false;
                }
                UserRoleModel.Add(userrole);
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            IdentityRole role = await roleManager.FindByIdAsync(RoleId);
            if (role == null)
            {
                return RedirectToPage("/NotFound");
            }
            IdentityResult result = null;
            foreach (UserForRole eachUserRole in UserRoleModel)
            {
                IdentityUser user = await userManager.FindByIdAsync(eachUserRole.UserId);

                if (eachUserRole.IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!eachUserRole.IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
            }

            if (result.Succeeded)
            {

                return RedirectToPage("EditRole", new { id = RoleId });
            }

            return Page();
        }
    }
}
