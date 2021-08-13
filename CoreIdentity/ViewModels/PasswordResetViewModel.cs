using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CoreIdentity.ViewModels
{
    public class PasswordResetViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email can't be null")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = " New Password")]
        [Required(ErrorMessage = "Password can't be null")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
