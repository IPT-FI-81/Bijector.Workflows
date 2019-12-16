using System;
using System.Linq;
using System.Threading.Tasks;
using Bijector.Infrastructure.Queues;
using Bijector.Infrastructure.Types;
using Bijector.Infrastructure.Types.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bijector.Workflows.Models
{
    public class CommandWorkflowNode: IWorkflowNode
    {
        private string type;

        private string commandType;

        public CommandWorkflowNode(){}

        public CommandWorkflowNode(ICommand command)
        {
            Action = command;
        }

        public object Action { get; set; }

        public string Type
        {
            get { return this.GetType().Name; }
            set { type = value; }
        }

        public string ActionType
        {
            get { return this.Action.GetType().Name; }
            set { commandType = value; }
        }

        public string ServiceName { get; set; }

        public int Id { get; set; }

        public async Task Execute(IContext context, IPublisher publisher, JObject parameters)
        {                        
            if(parameters != null)
            {
                foreach(JProperty jProperty in parameters.Properties())
                {
                    if(Action.GetType().GetProperty(jProperty.Name) != null)
                    {
                        dynamic parameter = jProperty.ToObject<dynamic>();
                        Action.GetType().GetProperty(jProperty.Name).SetValue(Action, parameter);
                    }
                }
            }

            dynamic command = Action;
            await publisher.Send(command, context);
        }

        public (int?, JObject) HandleRequest(IEvent @event, int next)
        {
            if(@event is IRejectedEvent)
            {
                var reason = (@event as IRejectedEvent).Reason;
                var jReason = JObject.Parse(JsonConvert.SerializeObject(new{reason = reason}));
                return (null, jReason);
            }
            return (next, JObject.Parse(JsonConvert.SerializeObject(@event)));
        }
    }
}   