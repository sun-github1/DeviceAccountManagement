﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DeviceCredentialManagement.Model
{
    public class DeviceAccountsBatch
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please provide an account name or sitename")]
        [Display(Name = "Site Name")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Please select an account type")]
        [Display(Name = "AccountType")]
        public AccountType? AccountFor { get; set; }


        [Required(ErrorMessage = "Please provide serial numbers for cameras")]
        [Display(Name = "List of SerialNumbers")]
        public string AccountReferenceNumbers { get; set; }

        [Required(ErrorMessage = "Please enter username")]
        public string Username { get; set; }


        //[Required(ErrorMessage = "Please enter or generate password")]
        public string Passwords { get; set; }
        public DateTime CreateUpdateDate { get; set; }
        public string LastRecordOperation { get; set; }
    }
}
