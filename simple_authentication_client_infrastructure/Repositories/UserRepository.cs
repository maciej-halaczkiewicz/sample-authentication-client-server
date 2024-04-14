using simple_authentication_client_application.Abstractions;
using simple_authentication_client_domain.Entities;
using simple_authentication_client_infrastructure.Context;
using simple_authentication_client_infrastructure.UnitOfWork;

namespace simple_authentication_client_infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(TemplateAppContext context) 
        : base(context)
    {
    }

    public IQueryable<User> GetAll()
    {
        return _context.Users;
    }

};