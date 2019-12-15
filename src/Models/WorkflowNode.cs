using System.Threading.Tasks;
using Bijector.Infrastructure.Queues;
using Bijector.Infrastructure.Types;
using Bijector.Infrastructure.Types.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bijector.Workflows.Models
{
    public interface IWorkflowNode
    {
        string ServiceName { get; set; }
        
        int Id { get; set; }

        Task Execute(IContext context, IPublisher publisher, JObject parameters);

        public (int?, JObject) HandleRequest(IEvent @event, int next);
    }

    public interface IStartWorkflowNode : IWorkflowNode
    {        
    }
}