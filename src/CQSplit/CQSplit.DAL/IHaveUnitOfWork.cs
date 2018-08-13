namespace CQSplit.DAL
{
    public interface IHaveUnitOfWork
    {
        IUnitOfWork UnitOfWork { get; set; }
    }
}