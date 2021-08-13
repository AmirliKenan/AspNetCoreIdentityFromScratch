using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CoreIdentity.Enums;

namespace CoreIdentity.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage ="UserName can't be null")]
        [Display(Name ="Name")]
        public string UserName { get; set; }
       
        [Display(Name = "Tel No")]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email can't be null")]
        [Display(Name = "EmailAddress")]
        [EmailAddress(ErrorMessage ="Email is wrong")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password can't be null")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string City { get; set; }
        public string Picture { get; set; }

        public DateTime? BirthDay { get; set; }
        public Gender Gender { get; set; }
    }
}
