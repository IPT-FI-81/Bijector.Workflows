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
    public class WorkflowNodeTimeStamps
    {
        public DateTimeOffset LastExecutionTime { get; set; }

        public DateTimeOffset NextExecutionTime { get; set; }

        public double IntervalInSeconds { get; set; }
    }

    public class TimeStartWorkflowNode : IStartWorkflowNode
    {
        private string type;

        private string actionType;

        public TimeStartWorkflowNode(){}

        public TimeStartWorkflowNode(DateTimeOffset nextExecutionTime, TimeSpan interval)
        {
            Action = new WorkflowNodeTimeStamps
            {
                LastExecutionTime = DateTimeOffset.MinValue,
                NextExecutionTime = nextExecutionTime,
                IntervalInSeconds = interval.TotalSeconds      
            };            
        }

        public object Action { get; set; }
        
        public string ServiceName { get; set; }
        
        public int Id { get; set; }
        
        public string Type 
        {
            get { return this.GetType().Name; }
            set { type = value; }
        }

        public string ActionType
        {
            get { return this.Action.GetType().Name; }
            set { actionType = value; }
        }

        public Task Execute(IContext context, IPublisher publisher, JObject parameters)
        {
            throw new System.NotImplementedException();
        }

        public (int?, JObject) HandleRequest(IEvent @event, int next)
        {
            if(@event is TimeArrivedEvent)
            {
                var time = (@event as TimeArrivedEvent).Time;
                if(time > (Action as WorkflowNodeTimeStamps).NextExecutionTime)
                {      
                    var json = JsonConvert.SerializeObject(new{time = time});
                    var jsonTime = JObject.Parse(json);
                    (Action as WorkflowNodeTimeStamps).LastExecutionTime = time;
                    (Action as WorkflowNodeTimeStamps).NextExecutionTime = time.AddSeconds((Action as WorkflowNodeTimeStamps).IntervalInSeconds);
                    return (next, jsonTime);
                }
            }
            if(@event is ForceStartEvent)
            {
                var jsonTime = JObject.Parse(JsonConvert.SerializeObject(DateTimeOffset.Now));
                return (next, jsonTime);
            }
            return (null, null);
        }
    }
}