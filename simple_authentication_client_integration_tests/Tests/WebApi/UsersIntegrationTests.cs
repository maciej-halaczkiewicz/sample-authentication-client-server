using FluentAssertions;
using NUnit.Framework;
using simple_authentication_client_application.Users.Queries.Dto;
using simple_authentication_client_integration_tests.Base;
using System.Net.Http.Json;
using simple_authentication_client_application.Common;

namespace simple_authentication_client_integration_tests.Tests.WebApi;

public class UsersIntegrationTests : IntegrationTestBase
{
    [Test, Property("UsersCount", "30")]
    public async Task ThatUserCanFetchUserList(CancellationToken cancellationToken)
    {
        await  SetupBearerHeaderForUserAsync(User1, cancellationToken);
        var userListResponse = await HttpClient.GetFromJsonAsync<ResponseWrapper<PageableList<UserDto>>>("/api/user-list", cancellationToken);

        userListResponse.Should().NotBeNull();
        userListResponse.Payload.Should().NotBeNull();
        userListResponse.Payload.Items.Count.Should().Be(10);
        userListResponse.Payload.TotalPages.Should().Be(3);
        userListResponse.Payload.TotalCount.Should().Be(30);
        userListResponse.Payload.HasNextPage.Should().BeTrue();
    }
}