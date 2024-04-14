using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace simple_authentication_client_api.Implementations;

public class CustomClaimsTransformation : IClaimsTransformation
{
    public CustomClaimsTransformation()
    {
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        ClaimsIdentity claimsIdentity = new ClaimsIdentity();
        // authorize with roles/etc here...

        principal.AddIdentity(claimsIdentity);
        return principal;
    }
}