namespace CQRSTutorial.Core
{
    public interface ICommandWithAggregateId : ICommand
    {
        int AggregateId { get; set; }
    }
}