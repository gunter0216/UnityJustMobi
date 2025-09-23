using System;
using UniRx;

namespace App.Common.Events.External
{
    public class EventManager : IEventManager
    {
        public IDisposable Subscribe<T>(Action<T> callback)
        {
            return MessageBroker.Default.Receive<T>().Subscribe(callback);
        }
        
        public void Trigger<T>(T value)
        {
            MessageBroker.Default.Publish<T>(value);
        }
    }
}