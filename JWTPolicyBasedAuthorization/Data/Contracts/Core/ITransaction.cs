using System;

namespace JWTPolicyBasedAuthorization.Data.Contracts
{
    public interface ITransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}