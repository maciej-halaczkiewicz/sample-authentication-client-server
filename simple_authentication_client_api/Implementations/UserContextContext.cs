using simple_authentication_client_application.Abstractions;
namespace simple_authentication_client_api.Implementations
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string UserName
        {
            get => _httpContextAccessor.HttpContext.User.Identity.Name;
        }
    }
}
