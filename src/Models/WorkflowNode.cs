using System.Threading.Tasks;
using Bijector.Infrastructure.Queues;
using Bijector.Infrastructure.Types;
using Bijector.Infrastructure.Types.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bijector.Workflows.Models
{
    public abstract class WorkflowNode
    {
        public string ServiceName { get; set; }
        
        public int Id { get; set; }

        public abstract Task Execute(IContext context, IPublisher publisher, JObject parameters);

        public abstract (int?, JObject) HandleRequest(IEvent @event, int next);
    }

    public abstract class StartWorkflowNode : WorkflowNode
    {
        public override Task Execute(IContext context, IPublisher publisher, JObject parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}