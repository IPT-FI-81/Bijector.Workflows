using System;
using Bijector.Infrastructure.Types.Messages;

namespace Bijector.Workflows.Messages.Events
{
    public class TimeArrivedEvent : IEvent
    {
        public DateTimeOffset Time { get; set; }
    }   
}