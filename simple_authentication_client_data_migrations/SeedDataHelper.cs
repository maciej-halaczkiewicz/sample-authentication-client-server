using Microsoft.EntityFrameworkCore;
using simple_authentication_client_application.Common;
using simple_authentication_client_domain.Entities;
using simple_authentication_client_infrastructure.Context;

namespace simple_authentication_client_migrations;

public class SeedDataHelper
{
    private readonly TemplateAppContext _context;

    public SeedDataHelper(TemplateAppContext context)
    {
        _context = context;
    }

    public async Task AddUser(string userName, int age)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user == null)
        {
            await _context.Users.AddAsync(new User() { 
                UserName = userName,
                FirstName = $"{userName}_{nameof(User.FirstName)}",
                LastName = $"{userName}_{nameof(User.LastName)}",
                Age = age,
                Password = userName.CalculateMD5(),
            });
            await _context.SaveChangesAsync();
        }
    }

}