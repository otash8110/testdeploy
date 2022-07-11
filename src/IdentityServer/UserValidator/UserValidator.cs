using IdentityServer4.Validation;
using IdentityServer4.Models;
using System.Security.Claims;
using Application.Interfaces.IServices;

namespace IdentityServer.UserValidator
{
    public class UserValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUsersService _userManager;
        public UserValidator(IUsersService userManager)
        {
            _userManager = userManager;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var userEmail = context.UserName;
            var userPassword = context.Password;

            var user = await _userManager.GetUserAsync(u => u.Email == userEmail);

            if (user != null)
            {
                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, userPassword);
                if (!isPasswordCorrect)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient,
                        "Invalid credentials");
                    return;
                } else
                {
                    var role = await _userManager.GetRolesAsync(user);
                    context.Result = new GrantValidationResult(
                    subject: user.Id.ToString(),
                    authenticationMethod: "custom",
                    claims: new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, role)
                    });

                    return;
                }
            }
        }
    }
}
