using System;
using System.Threading.Tasks;
using Bijector.Infrastructure.Queues;
using Bijector.Infrastructure.Types;
using Bijector.Infrastructure.Types.Messages;
using Bijector.Workflows.Messages.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bijector.Workflows.Models
{
    public class TimeConditonalWorkflowNode : StartWorkflowNode
    {
        public DateTimeOffset LastExecutionTime { get; set; }

        public DateTimeOffset NextExecutionTime { get; set; }

        public override (int?, JObject) HandleRequest(IEvent @event, int next)
        {
            if(@event is TimeArrivedEvent)
            {
                var time = (@event as TimeArrivedEvent).Time;
                if(time > NextExecutionTime)
                {
                    var jsonTime = JObject.Parse(JsonConvert.SerializeObject(time));
                    LastExecutionTime = time;
                    return (next, jsonTime);
                }
            }
            if(@event is ForceStartEvent)
            {
                var jsonTime = JObject.Parse(JsonConvert.SerializeObject(DateTimeOffset.Now));
                return (next, jsonTime);
            }
            return (Id, null);
        }
    }
}