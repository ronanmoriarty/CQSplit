using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Cafe.Domain;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;
using CQRSTutorial.Core;
using CQRSTutorial.DAL;

namespace Cafe.Waiter.Web.Controllers
{
    public class TabController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public TabController()
            : this(new CommandDispatcher(
                new EventReceiver(
                    new NHibernateUnitOfWorkFactory(SessionFactory.Instance),
                    new EventStore(SessionFactory.Instance, new EventMapper(typeof(TabOpened).Assembly)),
                    new EventToPublishRepository(SessionFactory.Instance, new EventToPublishMapper(typeof(TabOpened).Assembly))
                ),
                new AggregateStore(new List<ICommandHandler> { new TabFactory() }),
                new TypeInspector())
            )
        {
        }

        public TabController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        // GET: Tab
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create()
        {
            var openTabCommand = CreateOpenTabCommand();
            _commandDispatcher.Dispatch(openTabCommand);
            return RedirectToAction("Index", new {tabId = openTabCommand.AggregateId});
        }

        private OpenTab CreateOpenTabCommand()
        {
            return new OpenTab
            {
                Id = Guid.NewGuid(),
                AggregateId = Guid.NewGuid(),
                Waiter = "John",
                TableNumber = 5
            };
        }
    }
}