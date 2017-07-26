using System;
using CQRSTutorial.Core;

namespace CQRSTutorial.DAL.Tests
{
    internal class TestEvent2 : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
    }
}