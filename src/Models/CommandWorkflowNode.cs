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
        public CommandWorkflowNode(){}

        public CommandWorkflowNode(ICommand command)
        {
            Command = command;
        }

        public object Command { get; set; }

        //public string CommandType { get; set; }

        public string ServiceName { get; set; }

        public int Id { get; set; }

        public async Task Execute(IContext context, IPublisher publisher, JObject parameters)
        {
            /*Type objectType = (from asm in AppDomain.CurrentDomain.GetAssemblies()
                   from type in asm.GetTypes()
                   where type.IsClass && type.Name == CommandType
                   select type).Single();
            dynamic command = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(Command), objectType); */
            dynamic command = Command;
                 
            foreach(JProperty jProperty in parameters.Properties())
            {
                if(Command.GetType().GetProperty(jProperty.Name) != null)
                {
                    dynamic parameter = jProperty.ToObject<dynamic>();
                    Command.GetType().GetProperty(jProperty.Name).SetValue(Command, parameter);
                }
            }

            await publisher.Send(command, context);
        }

        public (int?, JObject) HandleRequest(IEvent @event, int next)
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