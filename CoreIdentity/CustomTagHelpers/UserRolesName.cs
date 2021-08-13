using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CoreIdentity.TagHelpers
{
    [HtmlTargetElement("td",Attributes ="user-roles")]
    public class UserRolesName:TagHelper
    {
        private readonly UserManager<AppUser> userManager;

        public UserRolesName(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        [HtmlAttributeName("user-roles")]
        public string UserId { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            AppUser user =await userManager.FindByIdAsync(UserId);

         IList<string> rol= await userManager.GetRolesAsync(user);
            string html = string.Empty;
            rol.ToList().ForEach(x =>
            {
                html += $"<span class='btn btn-danger'>{x}</span>";

            });

            output.Content.SetHtmlContent(html);
        }
    }
}
