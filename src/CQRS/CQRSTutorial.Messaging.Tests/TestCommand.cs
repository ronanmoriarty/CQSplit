using System;
using System.Diagnostics.CodeAnalysis;
using CQRSTutorial.Core;

namespace CQRSTutorial.Messaging.Tests
{
    [ExcludeFromCodeCoverage]
    public class TestCommand : ICommand
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
    }
}