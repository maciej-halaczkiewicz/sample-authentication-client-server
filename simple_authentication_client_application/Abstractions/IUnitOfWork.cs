namespace simple_authentication_client_application.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
        Task RollbackAsync();
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        TCustomRepository GetCustomRepository<TCustomRepository>()
            where TCustomRepository : class;
    }

}
