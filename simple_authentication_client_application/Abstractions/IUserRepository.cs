using simple_authentication_client_domain.Entities;

namespace simple_authentication_client_application.Abstractions;

public interface IUserRepository : IRepository<User>
{
    IQueryable<User> GetAll();
};