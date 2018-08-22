using System;
using System.Diagnostics.CodeAnalysis;

namespace CQSplit.Messaging.Tests
{
    [ExcludeFromCodeCoverage]
    public class TestCommand : ICommand
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
    }
}