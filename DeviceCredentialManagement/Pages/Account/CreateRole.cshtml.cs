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
    public class CreateRoleModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly RoleManager<IdentityRole> roleManager;
        [BindProperty]
        public CreateRole NewRole { get; set; }

        public CreateRoleModel(ILogger<CreateRoleModel> _logger, RoleManager<IdentityRole> roleManager)
        {
            this._logger = _logger;
            this.roleManager = roleManager;

        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if(ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name= NewRole.RoleName
                };

                var result=await roleManager.CreateAsync(role);
                if(result.Succeeded)
                {
                    return RedirectToPage("/Account/ListRoles");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return Page();
        }
    }
}
