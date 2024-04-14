using FluentAssertions;
using NUnit.Framework;
using simple_authentication_client_application.Users.Commands.Dto;
using simple_authentication_client_application.Users.Queries.Dto;
using simple_authentication_client_integration_tests.Base;
using simple_authentication_client_integration_tests.Implementations;
using System.Net.Http.Json;
using simple_authentication_client_application.Common;

namespace simple_authentication_client_integration_tests.Tests.WebApi;

public class AuthenticationIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task ThatUserCanFetchToken()
    {
        var authenticationResponse = await HttpClient.GetFromJsonAsync<ResponseWrapper<AuthenticationResponse>>($"/api/authenticate?username={TestBase.User1}&password={TestBase.User1}");

        authenticationResponse.Should().NotBeNull();
        authenticationResponse.IsSuccess.Should().BeTrue();
        authenticationResponse.Payload.Should().NotBeNull();
        authenticationResponse.Payload.Token.Should().NotBe(new Guid());
    }
    [Test]
    public async Task ThatWrongUserCannotFetchToken()
    {
        var authenticationResponse = await HttpClient.GetFromJsonAsync<ResponseWrapper<AuthenticationResponse>>($"/api/authenticate?username=abc&password=");

        authenticationResponse.Should().NotBeNull();
        authenticationResponse.IsSuccess.Should().BeFalse();
        authenticationResponse.ErrorMessage.Should().NotBeNull();
    }

    [Test]
    public async Task ThatUserCanPassAuthenticationWithToken(CancellationToken cancellationToken)
    {
        await SetupBearerHeaderForUserAsync(User1, cancellationToken);

        var result = await HttpClient.GetFromJsonAsync<ResponseWrapper<CurrentUserInfoResponse>>("/api/current-user-info", cancellationToken);

        result.Should().NotBeNull();
        result.Payload.Username.Should().Be(TestBase.User1);
    }

    [Test]
    public async Task ThatUserWillReceiveSameTokenWithinSameSession()
    {
        var authenticationResponse = await HttpClient.GetFromJsonAsync<ResponseWrapper<AuthenticationResponse>>($"/api/authenticate?username={TestBase.User1}&password={TestBase.User1}");

        authenticationResponse.Should().NotBeNull();
        authenticationResponse.IsSuccess.Should().BeTrue();
        authenticationResponse.Payload.Should().NotBeNull();
        authenticationResponse.Payload.Token.Should().NotBe(new Guid());

        var authenticationResponse2 = await HttpClient.GetFromJsonAsync<ResponseWrapper<AuthenticationResponse>>($"/api/authenticate?username={TestBase.User1}&password={TestBase.User1}");

        authenticationResponse2.Should().NotBeNull();
        authenticationResponse2.IsSuccess.Should().BeTrue();
        authenticationResponse2.Payload.Should().NotBeNull();

        authenticationResponse2.Payload.Token.Should().Be(authenticationResponse.Payload.Token);
    }

    [Test]
    public async Task ThatUserWillReceiveDifferentTokenWhenTokenExpires()
    {
        var now = DateTime.UtcNow;
        TestDateTimeProvider.GlobalNow = now;
        var authenticationResponse = await HttpClient.GetFromJsonAsync<ResponseWrapper<AuthenticationResponse>>($"/api/authenticate?username={TestBase.User1}&password={TestBase.User1}");

        authenticationResponse.Should().NotBeNull();
        authenticationResponse.IsSuccess.Should().BeTrue();
        authenticationResponse.Payload.Should().NotBeNull();
        authenticationResponse.Payload.Token.Should().NotBe(new Guid());

        TestDateTimeProvider.GlobalNow = now.AddMinutes(11);

        var authenticationResponse2 = await HttpClient.GetFromJsonAsync<ResponseWrapper<AuthenticationResponse>>($"/api/authenticate?username={TestBase.User1}&password={TestBase.User1}");

        authenticationResponse2.Should().NotBeNull();
        authenticationResponse2.IsSuccess.Should().BeTrue();
        authenticationResponse2.Payload.Should().NotBeNull();

        authenticationResponse2.Payload.Token.Should().NotBe(authenticationResponse.Payload.Token);
    }
}