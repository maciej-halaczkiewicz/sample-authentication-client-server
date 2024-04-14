using simple_authentication_client_application.Users.Queries.Dto;
using simple_authentication_client_console.Abstractions;
using System.Net;
using simple_authentication_client_application.Common;

namespace simple_authentication_client_console.Implementations;

public class ViewController : IViewController
{
    private enum State
    {
        Ok = 1,
        Unauthorized,
        Quit
    }
    private readonly IHttpService _httpService;
    public ViewController(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task MainLoop()
    {
        await HandleLogin();
        await HandleOptions();
    }

    private async Task HandleLogin()
    {
        bool loginResult = false;
        do
        {
            loginResult = await ShowLoginScreen();
        } while (!loginResult);
    }
    private async Task HandleOptions()
    {
        State optionsResult;
        do
        {
            optionsResult = await ShowOptionsScreen();
        } while (optionsResult == State.Ok);

        // token depreciated, start over
        if (optionsResult == State.Unauthorized)
        {
            await MainLoop();
        }
    }

    private async Task<bool> ShowLoginScreen()
    {
        Console.WriteLine("Provide username:");
        var userName = Console.ReadLine();

        Console.WriteLine("Provide password:");
        var password = Console.ReadLine();

        var (httpCode, authenticationUserResult) = await _httpService.AuthenticateUser(userName, password);

        if (httpCode == HttpStatusCode.OK && authenticationUserResult.IsSuccess)
        {
            Console.WriteLine("Hurray, you've logged in, please press any key to continue");
            Console.ReadKey();
            return true;
        }
        else if (httpCode == HttpStatusCode.OK && !authenticationUserResult.IsSuccess)
        {
            Console.WriteLine($"Login error, {authenticationUserResult.ErrorMessage}, please press any key and retry");
            Console.ReadKey();
            return false;
        }
        else
        {
            Console.WriteLine($"There was some unexpected error, please press any key and retry");
            Console.ReadKey();
            return false;
        }
    }
    private async Task<State> ShowOptionsScreen()
    {
        Console.WriteLine("Provide option: (1) Show current user details, (2) Show users list (first page), any other key to exit");
        var option = Console.ReadLine();

        switch (option)
        {
            case "1":
                {
                    var (httpCode, userDataResponse) = await _httpService.GetCurrentUserInfo();
                    return DisplayResult<ResponseWrapper<CurrentUserInfoResponse>, CurrentUserInfoResponse>(
                        httpCode,
                        userDataResponse,
                        (x) => $"Current user data: Username:{x.Payload.Username} FirstName:{x.Payload.FirstName}, SecondName: {x.Payload.LastName}, Age: {x.Payload.Age}");
                }
            case "2":
                {
                    var (httpCode, usersData) = await _httpService.GetUsersList();
                    return DisplayResult<ResponseWrapper<PageableList<UserDto>>, PageableList<UserDto>>(
                        httpCode,
                        usersData,
                        (x) => string.Join("\n", x.Payload.Items.Select(x => $"Username:{x.Username}")));
                }
            default: return State.Quit;
        }

    }

    private static State DisplayResult<T, TItem>(HttpStatusCode httpCode, T userDataResponse, Func<T, string> formatter)
        where T : ResponseWrapper<TItem>
        where TItem : class
    {
        if (httpCode == HttpStatusCode.OK)
        {
            if (userDataResponse.IsSuccess)
            {
                var userData = userDataResponse.Payload;
                Console.WriteLine(formatter(userDataResponse));
            }
            else
            {
                Console.WriteLine(userDataResponse.ErrorMessage);
            }
        }
        else if (httpCode == HttpStatusCode.Unauthorized)
        {
            return State.Unauthorized;
        }
        else
        {
            throw new Exception($"Error connecting to server code={httpCode}");
        }

        return State.Ok;
    }
}