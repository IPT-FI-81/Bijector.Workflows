using Bijector.Infrastructure.Types.Messages;

namespace Bijector.Workflows.Messages.Events
{
    public class MoveDriveEntityRejected : IRejectedEvent
    {
        public string EnitityId { get; set; }

        public string DestionationId { get; set; }

        public string Reason { get; set; }
    }
}