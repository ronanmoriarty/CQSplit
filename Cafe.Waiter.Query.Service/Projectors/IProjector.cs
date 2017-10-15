namespace Cafe.Waiter.EventProjecting.Service.Projectors
{
    public interface IProjector<in TEvent>
    {
        void Project(TEvent message);
    }
}