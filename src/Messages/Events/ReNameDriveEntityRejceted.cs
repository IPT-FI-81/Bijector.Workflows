using Bijector.Infrastructure.Types.Messages;

namespace Bijector.Workflows.Messages.Commands
{
    public class RenameDriveEntityRejected : IRejectedEvent
    {
        public string Id { get; set; }

        public string NewName { get; set; }

        public string Reason { get; set; }
    }
}