using System;
using System.Linq;
using System.Linq.Expressions;
using JWTPolicyBasedAuthorization.Data.Contracts;
using JWTPolicyBasedAuthorization.Data;
using Microsoft.EntityFrameworkCore;
using JWTPolicyBasedAuthorization.Models;

namespace JWTPolicyBasedAuthorization.Data.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class, IEntity
    {
        private ApplicationDbContext dbContext { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="dbContext"></param>
        public RepositoryBase(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public T FindById(int id)
        {
            return dbContext.Set<T>().FirstOrDefault(x => x.Id == id);
        }
        public IQueryable<T> FindAll()
        {
            return dbContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return dbContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Add(T entity)
        {
            dbContext.Set<T>().Add(entity);
        }

        public void AddRange(params T[] entites)
        {
            dbContext.Set<T>().AddRange(entites);
        }

        public void Update(T entity)
        {
            dbContext.Set<T>().Update(entity);
        }

        public void UpdateRange(params T[] entites)
        {
            dbContext.Set<T>().UpdateRange(entites);
        }

        public void Remove(T entity)
        {
            dbContext.Set<T>().Remove(entity);
        }

        public void RemoveRange(params T[] entites)
        {
            dbContext.Set<T>().RemoveRange(entites);
        }
    }
}