using System;

namespace CQSplit.Serialization
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void ExecuteInTransaction(Action action);
    }
}