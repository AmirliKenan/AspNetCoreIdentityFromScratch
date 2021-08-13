﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreIdentity.Models;
using Microsoft.AspNetCore.Identity;

namespace CoreIdentity.CustomValidation
{
    public class CustomUserValidation : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            string[] Digits = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            foreach (var item in Digits)
            {
                if (user.UserName[0].ToString() == item) 
                {

                    errors.Add(new IdentityError() { Code = "UserNameContainsFirstLetterDigitContains", Description = "User name don't be started with number" });
                }
            }

            if (errors.Count == 0)
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else 
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
          
           
        }
    }
}
