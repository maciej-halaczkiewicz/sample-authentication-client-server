namespace simple_authentication_client_domain.Entities;

public class UserSession
{
    public int Id { get; set; }
    public DateTime Expires { get; set; }
    public Guid Token { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;
}
