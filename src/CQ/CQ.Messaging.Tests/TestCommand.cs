using System;
using System.Diagnostics.CodeAnalysis;
using CQ.Core;

namespace CQ.Messaging.Tests
{
    [ExcludeFromCodeCoverage]
    public class TestCommand : ICommand
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
    }
}