using System;
using CQRSTutorial.Core;

namespace CQRSTutorial.Tests.Common
{
    public class TestEvent2 : IEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public Guid CommandId { get; set; }
    }
}