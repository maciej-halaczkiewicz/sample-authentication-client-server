using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using simple_authentication_client_application.Users.Queries;

namespace simple_authentication_client_api.Controllers
{
    [Authorize("UserPolicy")]
    public class CurrentUserInfoController : ApiControllerBase
    {
        [HttpGet("/api/current-user-info")]
        public async Task<IResult> Get()
        {
            var userInfo = await Mediator.Send(new GetCurrentUserInfo());
            return TypedResults.Ok(userInfo);
        }
    }
}
