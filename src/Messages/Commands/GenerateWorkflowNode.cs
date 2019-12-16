namespace Bijector.Workflows.Messages.Commands
{
    public class GenerateWorkflowNode
    {
        public GenerateWorkflowNode(){}

        public int Id { get; set; }

        public string Type { get; set; }

        public string ServiceName { get; set; }

        public string ActionType { get; set; }       

        public string ActionJson { get; set; }
    }
}