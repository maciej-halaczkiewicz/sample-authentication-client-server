using AutoMapper;
using simple_authentication_client_application.Abstractions;
using simple_authentication_client_domain.Entities;

namespace simple_authentication_client_application.Users.Queries.Dto
{
    public class CurrentUserInfoResponse : IMapFrom<User>
    {
        public string Username { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Age { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, CurrentUserInfoResponse>();
        }
    }
}
