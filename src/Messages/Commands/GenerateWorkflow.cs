using System.Collections.Generic;
using Bijector.Infrastructure.Types.Messages;

namespace Bijector.Workflows.Messages.Commands
{
    public class GenerateWorkflow : ICommand
    {        
        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<GenerateWorkflowNode> WorkflowNodes { get; set; }
    }
}