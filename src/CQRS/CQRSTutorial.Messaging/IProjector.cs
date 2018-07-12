﻿namespace CQRSTutorial.Messaging
{
    public interface IProjector<in TEvent>
    {
        void Project(TEvent message);
    }
}
