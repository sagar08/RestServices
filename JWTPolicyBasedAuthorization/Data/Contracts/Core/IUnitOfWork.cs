using System;
using System.Data;
using System.Threading.Tasks;

namespace JWTPolicyBasedAuthorization.Data.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        //ITransaction BeginTransaction(IsolationLevel level = IsolationLevel.Snapshot);
        IUserRepository User { get; }
        Task<int> SaveChangesAsync();
        //void CommitTransaction();
        //void RollbackTransaction();
    }
}