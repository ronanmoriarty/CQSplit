using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Cafe.Domain.Tests
{
    [TestFixture]
    public class CommandHandlerDictionaryTests
    {
        [Test]
        public void DictionaryWillThrowExceptionIfMoreThanOneHandlerTriesToHandleAnyGivenCommand()
        {
            Assert.That(() => new CommandHandlerDictionary(new object[] { new Handler1(), new Handler1() }), Throws.Exception.InstanceOf<ArgumentException>().With.Message.EqualTo($"More than one handler found for {typeof(TestCommand).FullName}"));
        }

        internal class Handler1 : ICommandHandler<TestCommand>
        {
            public IEnumerable<IEvent> Handle(TestCommand command)
            {
                throw new System.NotImplementedException();
            }
        }

        internal class TestCommand
        {
        }
    }
}