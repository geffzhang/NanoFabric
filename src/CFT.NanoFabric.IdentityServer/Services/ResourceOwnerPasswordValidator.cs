using System.Threading.Tasks;
using IdentityServer4.Validation;
using CFT.NanoFabric.IdentityServer.Interfaces.Services;
using CFT.NanoFabric.IdentityServer.Utilities;

namespace CFT.NanoFabric.IdentityServer.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public IUserService UserService { get; private set; }

        public ResourceOwnerPasswordValidator(IUserService userService)
        {
            UserService = userService;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await UserService.GetAsync(context.UserName, context.Password);

            if (user != null)
            {
                var claims = ClaimsUtility.GetClaims(user);
                context.Result = new GrantValidationResult(user.Id.ToString(), "password",claims);
            }
        }
    }
}
