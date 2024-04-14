using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using simple_authentication_client_application.Users.Queries;

namespace simple_authentication_client_api.Controllers
{
    [Authorize("UserPolicy")]
    public class UsersController : ApiControllerBase
    {
        [HttpGet("/api/user-list")]
        public async Task<IResult> Get()
        {
            var users = await Mediator.Send(new GetAllUsers());
            return TypedResults.Ok(users);
        }
    }
}
