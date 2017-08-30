﻿using CQRSTutorial.DAL;

namespace Cafe.Waiter.DAL.Tests
{
    public class ReadModelConnectionStringProviderFactory
    {
        public static ConnectionStringProviderFactory Instance => new ConnectionStringProviderFactory("CQRSTutorial.Cafe.Waiter", "CQRSTUTORIAL_CAFE_WAITER_READMODEL_CONNECTIONSTRING_OVERRIDE");
    }
}