namespace Workflows.Models
{
    public class WorkflowNode
    {
        public WorkflowNode(IAction action, int nextNodeId)
        {
            Action = action;
            NextNodeId = nextNodeId;
        }

        public IAction Action { get; private set; }

        public int NextNodeId { get; private set; }
    }
}