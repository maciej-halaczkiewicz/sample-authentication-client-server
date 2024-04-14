using Newtonsoft.Json;
using Polly;
using simple_authentication_client_application.Users.Commands.Dto;
using simple_authentication_client_application.Users.Queries.Dto;
using simple_authentication_client_console.Abstractions;
using System.Net;
using System.Net.Http.Headers;
using simple_authentication_client_application.Common;

namespace simple_authentication_client_console.Implementations;


public class HttpService : IHttpService
{
    public readonly HttpClient _httpClient;
    private Guid? token = null;

    public HttpService(string webApiUrl)
    {
        // todo could have webApiUrl format validation
        var handler = new HttpClientHandler();
        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        // todo add certificate for docker container, for now turn of cert validation
        handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
        _httpClient = new HttpClient(handler);
        _httpClient.BaseAddress = new Uri(webApiUrl);
    }

    private async Task<(HttpStatusCode, T)> GetAsync<T>(string path)
        where T : class
    {
        var policyWithBackoffDelay = Policy
            .HandleResult<HttpResponseMessage>(r => r.StatusCode != HttpStatusCode.OK)
            .WaitAndRetryAsync(
                2,// attempts
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(5, retryAttempt)) // 5-second-based backoff exponential waiting logic
            );

        var httpResponse = await policyWithBackoffDelay.ExecuteAsync(async () =>
        {
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.ToString());
            }
            return await _httpClient.GetAsync(path);
        });
        if (httpResponse.StatusCode == HttpStatusCode.OK)
        {
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(stringResponse);
            return (httpResponse.StatusCode, result);
        }
        return (httpResponse.StatusCode, null);
    }

    public async Task<(HttpStatusCode, ResponseWrapper<AuthenticationResponse>)> AuthenticateUser(string userName, string password)
    {
        var (httpCode, result) = await GetAsync<ResponseWrapper<AuthenticationResponse>>($"/api/authenticate?username={userName}&password={password}");
        if (httpCode == HttpStatusCode.OK)
        {
            if (result.IsSuccess)
            {
                token = result.Payload.Token;
            }
        }
        else
        {
            throw new Exception("Authentication response error");
        }

        return (httpCode,result);
    }

    public async Task<(HttpStatusCode, ResponseWrapper<CurrentUserInfoResponse>)> GetCurrentUserInfo()
        => await GetAsync<ResponseWrapper<CurrentUserInfoResponse>>("/api/current-user-info");

    public async Task<bool> AuthenticateAsync(string userName, string password)
    {
        var (httpCode, result) = await AuthenticateUser(userName, password);
        if (httpCode == HttpStatusCode.OK)
        {
            if (result.IsSuccess)
            {
                token = result.Payload.Token;
            }
        }
        else
        {
            throw new Exception("Authentication response error");
        }

        return result.IsSuccess;
    }

    public async Task<(HttpStatusCode, ResponseWrapper<PageableList<UserDto>>)> GetUsersList()
        => await GetAsync<ResponseWrapper<PageableList<UserDto>>>("/api/user-list");
}