using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DeviceCredentialManagement.Model
{
    public class CreateRole
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
