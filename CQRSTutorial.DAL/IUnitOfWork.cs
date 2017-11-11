using System;

namespace CQRSTutorial.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        void Start();
        void Commit();
        void Rollback();
        void Enlist(params IHaveUnitOfWork[] haveUnitOfWorks);
    }
}