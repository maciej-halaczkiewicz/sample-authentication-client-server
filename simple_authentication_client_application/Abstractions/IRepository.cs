using System.Linq.Expressions;

namespace simple_authentication_client_application.Abstractions
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public IQueryable<TEntity> GetAll(); 

        public Task<TEntity> GetByIdAsync(object id);

        public void Add(TEntity entity);

        public void Delete(object id);

        public void Delete(TEntity entityToDelete);

        public void Update(TEntity entityToUpdate);
    }
}