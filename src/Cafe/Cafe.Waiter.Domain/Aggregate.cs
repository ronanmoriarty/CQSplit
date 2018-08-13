using System;
using CQSplit.Core;

namespace Cafe.Waiter.Domain
{
    public abstract class Aggregate
    {
        public Guid Id { get; set; }

        public bool CanHandle(ICommand command)
        {
            return GetType().CanHandle(command, Id);
        }
    }
}