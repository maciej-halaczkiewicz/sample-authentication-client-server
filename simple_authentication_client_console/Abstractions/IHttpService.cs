using simple_authentication_client_application.Users.Commands.Dto;
using simple_authentication_client_application.Users.Queries.Dto;
using System.Net;
using simple_authentication_client_application.Common;

namespace simple_authentication_client_console.Abstractions;

public interface IHttpService
{
    Task<(HttpStatusCode, ResponseWrapper<CurrentUserInfoResponse>)> GetCurrentUserInfo();
    Task<(HttpStatusCode, ResponseWrapper<PageableList<UserDto>>)> GetUsersList();
    Task<(HttpStatusCode, ResponseWrapper<AuthenticationResponse>)> AuthenticateUser(string userName, string password);
}