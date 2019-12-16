using Bijector.Infrastructure.Types.Messages;

namespace Bijector.Workflows.Messages.Events
{
    public class DriveEntityRenamed : IEvent
    {
        public string EntityId { get; set; }
        public string NewName { get; set; }
    }
}