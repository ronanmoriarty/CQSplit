using CQRSTutorial.Core;

namespace Cafe.Domain
{
    public abstract class Aggregate
    {
        public int Id { get; set; }

        public bool CanHandle(ICommand command)
        {
            return GetType().CanHandle(command, Id);
        }
    }
}