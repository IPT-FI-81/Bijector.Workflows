using Bijector.Infrastructure.Types.Messages;

namespace Bijector.Workflows.Messages.Events
{
    public class DriveEntityMoved : IEvent
    {
        public string EnitityId { get; set; }        

        public string DestionationId { get; set; }
    }
}