using System;
using CQRSTutorial.Core;

namespace CQRSTutorial.DAL.Tests
{
    public class TestEvent2 : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
    }
}