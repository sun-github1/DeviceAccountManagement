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
    public class ListRolesModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly RoleManager<IdentityRole> roleManager;
        [BindProperty]
        public List<IdentityRole> ListofRoles { get; set;}

        public ListRolesModel(ILogger<ListRolesModel> _logger, RoleManager<IdentityRole> roleManager)
        {
            this._logger = _logger;
            this.roleManager = roleManager;
        }
        public IActionResult OnGet()
        {
            var listofroles = roleManager.Roles;
            ListofRoles = listofroles != null ? listofroles.ToList() : null;
            return Page();
        }
    }
}
