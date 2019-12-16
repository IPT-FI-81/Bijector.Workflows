using System.Collections.Generic;
using Bijector.Infrastructure.Types.Messages;
using Bijector.Workflows.Models;

namespace Bijector.Workflows.Messages.Commands
{
    public class UpdateWorkflow : ICommand
    {        
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<GenerateWorkflowNode> WorkflowNodes { get; set; }
    }
}