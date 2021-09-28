using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCredentialManagement.Model
{
    public class UserForRole
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; }
    }
}
