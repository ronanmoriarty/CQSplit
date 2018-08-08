﻿using System;
using NUnit.Framework;

namespace CQRSTutorial.Core.Tests
{
    [TestFixture]
    public class EventApplierTests
    {
        [Test]
        public void Calls_apply_with_supplied_event()
        {
            var eventApplier = new EventApplier(new TypeInspector());
            var testAggregate = new TestAggregate();
            var testEvent = new TestEvent();

            eventApplier.ApplyEvent(testEvent, testAggregate);

            Assert.That(testAggregate.AppliedEvent, Is.EqualTo(testEvent));
        }

        private class TestAggregate : IApplyEvent<TestEvent>
        {
            public void Apply(TestEvent @event)
            {
                AppliedEvent = @event;
            }

            public void SomeOtherMethodTakingSameSingleTestEventParameter(TestEvent @event)
            {
                // included to ensure EventApplier can select the right method.
            }

            public void Apply()
            {
                // included to ensure EventApplier can select the right method.
            }

            public void Apply(int i)
            {
                // included to ensure EventApplier can select the right method.
            }

            public TestEvent AppliedEvent { get; private set; }
        }

        internal class TestEvent : IEvent
        {
            public Guid Id { get; set; }
            public Guid AggregateId { get; set; }
            public Guid CommandId { get; set; }
        }
    }
}