namespace Cafe.Waiter.Query.Service.Projectors
{
    public interface IProjector<in TEvent>
    {
        void Project(TEvent message);
    }
}