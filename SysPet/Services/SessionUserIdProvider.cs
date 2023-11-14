namespace SysPet.Services
{
    using Microsoft.AspNetCore.Http;

    public class SessionUserIdProvider : IUserIdProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionUserIdProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetUserId()
        {
            return _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");
        }
    }

}
