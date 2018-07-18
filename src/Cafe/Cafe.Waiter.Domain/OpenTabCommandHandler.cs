﻿using System;
using System.Collections.Generic;
using Cafe.Waiter.Contracts.Commands;
using Cafe.Waiter.Events;
using CQRSTutorial.Core;
using log4net;

namespace Cafe.Waiter.Domain
{
    public class OpenTabCommandHandler : ICommandHandler<IOpenTabCommand>
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(OpenTabCommandHandler));

        public IEnumerable<IEvent> Handle(IOpenTabCommand command)
        {
            var eventId = Guid.NewGuid();
            _logger.Debug($"Creating event {eventId} for command {command.Id} on aggregate {command.AggregateId}");
            return new IEvent[]
            {
                new TabOpened
                {
                    Id = eventId,
                    CommandId = command.Id,
                    AggregateId = command.AggregateId,
                    TableNumber = command.TableNumber,
                    Waiter = command.Waiter
                }
            };
        }

        public bool CanHandle(ICommand command)
        {
            return command is IOpenTabCommand;
        }
    }
}