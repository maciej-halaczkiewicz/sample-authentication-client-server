using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using simple_authentication_client_application.Users.Queries;
using System.Security.Claims;
using System.Text.Encodings.Web;
using simple_authentication_client_application.Users.Commands;

namespace simple_authentication_client_api.Implementations;

public class CustomBearerTokenAuthSchemeHandler : AuthenticationHandler<CustomBearerTokenAuthSchemeOptions>
{
    public const string AuthenticationScheme = "CustomBearer";
    private const string AuthorizationHeaderName = "Authorization";
    private readonly ISender _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomBearerTokenAuthSchemeHandler(
        IOptionsMonitor<CustomBearerTokenAuthSchemeOptions> options,
        ILoggerFactory logger,
        IHttpContextAccessor httpContextAccessor,
        ISender mediator,
        UrlEncoder encoder) : base(options, logger, encoder)
    {
        _httpContextAccessor = httpContextAccessor;
        _mediator = mediator;
    }

    protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (_httpContextAccessor.HttpContext != null 
            && _httpContextAccessor.HttpContext.Request.Headers.TryGetValue(AuthorizationHeaderName, out var header))
        {
            var authenticationHeader = header.ToString();
            if (authenticationHeader.StartsWith("Bearer "))
            {
                var tokenString = authenticationHeader.Replace("Bearer ", string.Empty);
                if (Guid.TryParse(tokenString, out var tokenGuid))
                {
                    var session = await _mediator.Send(new ProlongSessionByToken(tokenGuid));
                    if (session != null)
                    {
                        var claims = new[] { new Claim(ClaimTypes.Name, session.UserName) };
                        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Bearer"));
                        var ticket = new AuthenticationTicket(principal, Scheme.Name);
                        return AuthenticateResult.Success(ticket);
                    }
                }
            }
            return AuthenticateResult.Fail("Invalid token");
        }
        return AuthenticateResult.Fail("Token not present");
    }
}