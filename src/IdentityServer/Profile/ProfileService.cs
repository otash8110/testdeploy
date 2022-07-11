using Application.Interfaces.IServices;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityServer.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly IUsersService _usersService;

        public ProfileService(IUsersService usersService)
        {
            _usersService = usersService;

        }
        /// <summary>
        /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var id = context.Subject.GetSubjectId();
            var user = await _usersService.GetUserByIdAsync(Convert.ToInt32(id));
            var role = await _usersService.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim("Role", role),
            };

            context.IssuedClaims.AddRange(claims);

            return;
        }

        /// <summary>
        /// This method gets called whenever identity server needs to determine if the user is valid or active (e.g. if the user's account has been deactivated since they logged in).
        /// (e.g. during token issuance or validation).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
