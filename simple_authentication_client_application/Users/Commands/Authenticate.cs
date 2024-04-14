using FluentValidation;
using MediatR;
using simple_authentication_client_application.Common;
using simple_authentication_client_application.Users.Commands.Dto;

namespace simple_authentication_client_application.Users.Commands
{
    public class Authenticate : IRequest<ResponseWrapper<AuthenticationResponse>>
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }

    public class AuthenticateValidator : AbstractValidator<Authenticate>
    {
        public AuthenticateValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Please specify a username"); ;
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please specify a password");
        }
    }
}
