namespace Cafe.Domain
{
    public interface ICommandHandler<in TCommand>
    {
        void Handle(TCommand command);
    }
}