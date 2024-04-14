using simple_authentication_client_application.Abstractions;
using simple_authentication_client_domain.Entities;

namespace simple_authentication_client_application.Users.Queries.Dto
{
    public class UserDto : IMapFrom<User>
    {
        public string Username { get; set; } = null!;
    }
}
