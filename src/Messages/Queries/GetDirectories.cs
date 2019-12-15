using Bijector.Infrastructure.Types.Messages;
using Bijector.Workflows.Messages.DTOs;

namespace Bijector.Workflows.Messages.Queries
{
    public class GetDirectories : BaseDriveEntityMessage, IQuery<DriveEntityList>
    {
        public GetDirectories(){}

        public GetDirectories(int serviceId, string name) : base(serviceId)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}