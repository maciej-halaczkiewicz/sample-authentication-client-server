using simple_authentication_client_application.Abstractions;
using simple_authentication_client_infrastructure.Context;

namespace simple_authentication_client_infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly TemplateAppContext _context;
    private readonly IServiceProvider _serviceProvider;
    private Dictionary<Type, object> _repositories;

    public UnitOfWork(TemplateAppContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
        _serviceProvider = serviceProvider;
    }


    // todo add isolation level overrides and transaction support
    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task RollbackAsync()
    {
    }

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        if (_repositories.ContainsKey(typeof(TEntity)))
        {
            return (IRepository<TEntity>)_repositories[typeof(TEntity)];
        }

        var repository = new Repository<TEntity>(_context);
        _repositories.Add(typeof(TEntity), repository);
        return repository;
    }
    public TCustomRepository GetCustomRepository<TCustomRepository>() 
        where TCustomRepository: class
    {
        if (_repositories.ContainsKey(typeof(TCustomRepository)))
        {
            return (TCustomRepository)_repositories[typeof(TCustomRepository)];
        }
        var repository = (TCustomRepository)_serviceProvider.GetService(typeof(TCustomRepository));
        _repositories.Add(typeof(TCustomRepository), repository);
        return repository;
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

