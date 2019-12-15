using Bijector.Infrastructure.Types.Messages;
using Bijector.Workflows.Messages.DTOs;

namespace Bijector.Workflows.Messages.Queries
{
    public class GetFiles : BaseDriveEntityMessage, IQuery<DriveEntityList>
    {
        public GetFiles(){}

        public GetFiles(int serviceId, string name) : base(serviceId)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}