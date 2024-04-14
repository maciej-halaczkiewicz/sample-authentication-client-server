namespace simple_authentication_client_domain.Entities;

public class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
    public string Password { get; set; } = null!;
    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
}
