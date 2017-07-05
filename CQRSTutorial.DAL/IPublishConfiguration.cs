using System;

namespace CQRSTutorial.DAL
{
    public interface IPublishConfiguration
    {
        string GetPublishLocationFor(Type typeToPublish);
    }
}