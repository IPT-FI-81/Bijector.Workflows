using System.Collections.Generic;
using Bijector.Infrastructure.Types;

namespace Bijector.Workflows.Models
{
    public class Workflow : IIdentifiable
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public int CurrentNodeId { get; set; }

        public WorkflowState State { get; set; }

        public List<IWorkflowNode> WorkflowNodes { get; set; }
    }
}