namespace SysPet.Services
{
    using Microsoft.AspNetCore.Identity;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class UserService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public string GetUserIdAsync(ClaimsPrincipal user)
        {
            var userId = _userManager.GetUserId(user);
            return userId;
        }

        public async Task<string> GetUserIdAsync(IdentityUser user)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            return userId;
        }
    }

}
