using System;
using Vantage.Common.Utility;
using Vantage.WPF.Interfaces;

namespace Vantage.WPF.Services
{
    public class MessagingService : IMessagingService
    {
        public void Send<TMessage>(TMessage message, object sender = null) where TMessage : IMessage
        {
            if (sender == null)
                Messenger.Default.Send<TMessage>(message);
            else
                Messenger.Default.Send<TMessage>(message, sender);
        }

        public void Subscribe<TMessage>(object subscriber, Action<object, TMessage> callback) where TMessage : IMessage
        {
            Messenger.Default.Register<TMessage>(subscriber, (obj) => { callback.Invoke(subscriber, obj); });
        }

        public void Unsubscribe<TMessage>(object subscriber) where TMessage : IMessage
        {
            Messenger.Default.Unregister(subscriber);
        }
    }
}
