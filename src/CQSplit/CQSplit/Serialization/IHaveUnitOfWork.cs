namespace CQSplit.Serialization
{
    public interface IHaveUnitOfWork
    {
        IUnitOfWork UnitOfWork { get; set; }
    }
}