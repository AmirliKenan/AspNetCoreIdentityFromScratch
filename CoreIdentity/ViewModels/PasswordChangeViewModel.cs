using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreIdentity.ViewModels
{
    public class PasswordChangeViewModel
    {
    [Display(Name ="Old Password")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage ="Please add old password")]
        public string PasswordOld { get; set; }
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please add new password")]
        public string PasswordNew { get; set; }
        [Display(Name = "New Password again")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please add new password again")]
        [Compare("PasswordNew",ErrorMessage ="your confirm password don't match new password")]
        public string PasswordConfirm { get; set; }
    }
}
