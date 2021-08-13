using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreIdentity.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name ="Email")]
        [Required(ErrorMessage ="Email can't be null")]
      [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password can't be null")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
