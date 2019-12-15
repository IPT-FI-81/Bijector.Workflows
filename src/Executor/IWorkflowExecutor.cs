using System.Collections;
using System.Threading.Tasks;
using Bijector.Infrastructure.Types;
using Bijector.Infrastructure.Types.Messages;
using Bijector.Workflows.Models;

namespace Bijector.Workflows.Executor
{
    public interface IWorkflowExecutor
    {
        Task<Workflow> GetWorkflow(int id);

        Task<bool> Handle(IEvent @event, IContext context);
    }
}