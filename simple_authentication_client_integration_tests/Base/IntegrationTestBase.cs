using NUnit.Framework;
using simple_authentication_client_application.Users.Commands.Dto;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using simple_authentication_client_application.Common;

namespace simple_authentication_client_integration_tests.Base;

public class IntegrationTestBase : TestBase
{
    [SetUp]
    public virtual async Task TestSetUp()
    {
        await Initialize();
    }
    [TearDown]
    public virtual async Task TestTearDown()
    {
        await StopAsync();
    }

    protected async Task SetupBearerHeaderForUserAsync(string username, CancellationToken cancellationToken)
    {
        var authenticationResponse = await HttpClient.GetFromJsonAsync<ResponseWrapper<AuthenticationResponse>>($"/api/authenticate?username={username}&password={username}", cancellationToken);
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResponse.Payload.Token.ToString());
    }
}