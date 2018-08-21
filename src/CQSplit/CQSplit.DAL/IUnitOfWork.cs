using System;

namespace CQSplit.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void ExecuteInTransaction(Action action);
    }
}