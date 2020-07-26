using System;
using System.Linq;
using System.Linq.Expressions;

namespace JWTPolicyBasedAuthorization.Data.Contracts
{
    public interface IRepositoryBase<T>
    {
        T FindById(int id);

        IQueryable<T> FindAll();

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);

        void Add(T entity);
        void AddRange(params T[] entites);

        void Update(T entity);
        void UpdateRange(params T[] entites);

        void Remove(T entity);
        void RemoveRange(params T[] entites);
    }
}