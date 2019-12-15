using Bijector.Infrastructure.Types.Messages;

namespace Bijector.Workflows.Messages.Commands
{
    public class MoveDriveEntity : BaseDriveEntityMessage, ICommand
    {
        public MoveDriveEntity(){}

        public MoveDriveEntity(int serviceId, string entityId, string destinationId, string sourceId) : base(serviceId)
        {            
            EntityId = entityId;
            DestinationId = destinationId;
            SourceId = sourceId;
        }
        
        public string EntityId { get; set; }
        public string DestinationId { get; set; }
        public string SourceId { get; }
    }
}