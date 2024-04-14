using AutoMapper;
using simple_authentication_client_application.Abstractions;
using simple_authentication_client_domain.Entities;

namespace simple_authentication_client_application.Users.Commands.Dto
{
    public class SessionResponse : IMapFrom<UserSession>
    {
        public string UserName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserSession, SessionResponse>()
                .ForMember(x => x.UserName, x => x.MapFrom(y => y.User.UserName));
        }
    }
}
