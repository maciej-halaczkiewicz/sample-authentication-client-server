using simple_authentication_client_domain.Entities;
using simple_authentication_client_infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using simple_authentication_client_application.Common;

namespace simple_authentication_client_infrastructure;

public class SeedDataHelper
{
    private readonly TemplateAppContext _context;

    public SeedDataHelper(TemplateAppContext context)
    {
        _context = context;
    }

    public async Task AddUser(string username, int age)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        if (user == null)
        {
            await _context.Users.AddAsync(new User() { UserName = username, FirstName = $"{username}_{nameof(User.FirstName)}", LastName = $"{username}_{nameof(User.LastName)}", Age = age, Password = username.CalculateMD5() });
            await _context.SaveChangesAsync();
        }
    }
}