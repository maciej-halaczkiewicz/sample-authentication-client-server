using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using simple_authentication_client_application.Users.Commands;

namespace simple_authentication_client_api.Controllers
{
    [AllowAnonymous]
    public class AuthenticateController : ApiControllerBase
    {
        [HttpGet("/api/authenticate")]
        public async Task<IResult> Authenticate(string? userName, string? password)
        {
            var authenticationResult = await Mediator.Send(new Authenticate { UserName = userName, Password = password });
            return TypedResults.Ok(authenticationResult);
        }
    }
}
