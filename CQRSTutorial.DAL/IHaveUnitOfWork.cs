namespace CQRSTutorial.DAL
{
    public interface IHaveUnitOfWork
    {
        IUnitOfWork UnitOfWork { get; set; }
    }
}