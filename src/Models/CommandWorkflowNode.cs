using System.Threading.Tasks;
using Bijector.Infrastructure.Queues;
using Bijector.Infrastructure.Types;
using Bijector.Infrastructure.Types.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bijector.Workflows.Models
{
        public class CommandWorkflowNode : WorkflowNode
    {
        public CommandWorkflowNode(ICommand command)
        {
            Command = command;
        }

        public ICommand Command { get; }

        public async override Task Execute(IContext context, IPublisher publisher, JObject parameters)
        {
            foreach(JProperty jProperty in parameters.Properties())
            {
                if(Command.GetType().GetProperty(jProperty.Name) != null)
                {
                    dynamic parameter = jProperty.ToObject<dynamic>();
                    Command.GetType().GetProperty(jProperty.Name).SetValue(Command, parameter);
                }
            }
            await publisher.Send(Command, context);
        }

        public override (int?, JObject) HandleRequest(IEvent @event, int next)
        {
            if(@event is IRejectedEvent)
            {
                var reason = (@event as IRejectedEvent).Reason;
                var jReason = JObject.Parse(JsonConvert.SerializeObject(reason));
                return (null, jReason);
            }
            return (next, JObject.Parse(JsonConvert.SerializeObject(@event)));
        }
    }
}   