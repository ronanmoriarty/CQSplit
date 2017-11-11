using System;

namespace CQRSTutorial.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        void Start();
        void Commit();
        void Rollback();
        void Enroll(params IHaveUnitOfWork[] haveUnitOfWorks);
        IUnitOfWork Enrolling(params IHaveUnitOfWork[] haveUnitOfWorks);
        void Execute(Action action);
    }
}