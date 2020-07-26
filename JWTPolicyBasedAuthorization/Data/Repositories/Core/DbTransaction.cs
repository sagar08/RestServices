using JWTPolicyBasedAuthorization.Data.Contracts;
using Microsoft.EntityFrameworkCore.Storage;

namespace JWTPolicyBasedAuthorization.Data.Repositories
{
    public class DbTransaction : ITransaction
    {
        private readonly IDbContextTransaction _transaction;

        public DbTransaction(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
}