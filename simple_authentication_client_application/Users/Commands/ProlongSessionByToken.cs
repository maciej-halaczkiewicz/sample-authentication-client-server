using MediatR;
using simple_authentication_client_application.Users.Commands.Dto;

namespace simple_authentication_client_application.Users.Commands
{
    public class ProlongSessionByToken : IRequest<SessionResponse>
    {
        public ProlongSessionByToken(Guid token)
        {
            Token = token;
        }
        public Guid Token { get; set; }
    }
}
