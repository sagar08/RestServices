using System.Data;
using JWTPolicyBasedAuthorization.Data.Contracts;
using JWTPolicyBasedAuthorization.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace JWTPolicyBasedAuthorization.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        //private ITransaction _transaction;
        private ApplicationDbContext _dbContext;
        private IUserRepository _users;
        private IRoleRepository _roles;
        private IProductRepository _products;

        IUserRepository IUnitOfWork.Users => _users ?? new UserRepository(_dbContext);
        IRoleRepository IUnitOfWork.Roles => _roles ?? new RoleRepository(_dbContext);
        IProductRepository IUnitOfWork.Products => _products ?? new ProductRepository(_dbContext);

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        // public ITransaction BeginTransaction(IsolationLevel level = IsolationLevel.Snapshot)
        // {
        //     _transaction =  new DbTransaction(_dbContext.Database.BeginTransaction(level));
        //     return _transaction;
        // }

        // public void CommitTransaction()
        // {
        //     _transaction.Commit();
        // }

        // public void RollbackTransaction()
        // {
        //     _transaction.Rollback();
        // }

        public void Dispose()
        {
            _dbContext = null;            
            GC.SuppressFinalize(_dbContext);
        }        
    }
}