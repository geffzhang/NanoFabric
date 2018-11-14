using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using NanoFabric.IdentityServer.Interfaces.Services;
using NanoFabric.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserService _userManager;

        public ProfileService(IUserService userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            string subject = context.Subject.Claims.ToList().Find(s => s.Type == "sub").Value;
            try
            {
                // Get Claims From Database, And Use Subject To Find The Related Claims, As A Subject Is An Unique Identity Of User
                List<string> claimStringList = new List<string>();
                if (claimStringList == null)
                {
                    return ;
                }
                else
                {
                    List<Claim> claimList = new List<Claim>();
                    for (int i = 0; i < claimStringList.Count; i++)
                    {
                        claimList.Add(new Claim("role", claimStringList[i]));
                    }
                    context.IssuedClaims = claimList.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                    return;
                }
            }
            catch
            {
                
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            if (int.TryParse(sub, out int userId))
            {
                var user = await _userManager.GetAsync(userId);

                context.IsActive = user != null;
            }
            else
            {
                context.IsActive = false;
            }
        }
    }

    internal static class UserExtensions
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
                new Claim(JwtClaimTypes.PreferredUserName, user.Username),
                new Claim(JwtClaimTypes.Name, user.Username)
            };

            return claims;
        }
    }
}

