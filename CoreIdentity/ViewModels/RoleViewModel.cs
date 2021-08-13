using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreIdentity.ViewModels
{
    public class RoleViewModel
    {[Display(Name ="Role Name")]
        [Required(ErrorMessage ="Role Name is required")]
        public string Name { get; set; }
        public string Id { get; set; }
    }
}
