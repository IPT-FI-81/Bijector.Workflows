using Bijector.Infrastructure.Types.Messages;
using Bijector.Workflows.Messages.DTOs;

namespace Bijector.Workflows.Messages.Queries
{
    public class GetDriveEntity : BaseDriveEntityMessage, IQuery<DriveEntity>
    {
        public GetDriveEntity(){}

        public GetDriveEntity(int serviceId, string id) : base(serviceId)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}