namespace simple_authentication_client_application.Users.Queries;

public class PageableRequest
{
    public int PageNumber { get; set; } = 10;
    public int PageSize { get; } = 1;
}