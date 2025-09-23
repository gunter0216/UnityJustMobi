using System;

namespace App.Common.Events.External
{
    public interface IEventManager
    {
        IDisposable Subscribe<T>(Action<T> callback);
        void Trigger<T>(T value);
    }
}