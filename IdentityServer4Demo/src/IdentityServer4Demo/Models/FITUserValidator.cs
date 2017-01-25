using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4Demo.Models
{
    public class FITUserValidator : IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (context.UserName == "bob" && context.Password == "bob")
            {
                //使用subject可用于在资源服务器区分用户身份等等
                //获取：通过User.Claims.Where(l => l.Type == "sub").FirstOrDefault();获取
                context.Result = new GrantValidationResult(subject: "admin", authenticationMethod: "custom");
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }
            return Task.FromResult(0);
        }
    }
}
