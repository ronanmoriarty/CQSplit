namespace CQ.DAL
{
    public interface IHaveUnitOfWork
    {
        IUnitOfWork UnitOfWork { get; set; }
    }
}