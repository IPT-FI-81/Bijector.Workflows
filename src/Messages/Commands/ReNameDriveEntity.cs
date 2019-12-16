using Bijector.Infrastructure.Types.Messages;

namespace Bijector.Workflows.Messages.Commands
{
    public class RenameDriveEntity : BaseDriveEntityMessage, ICommand
    {
        public RenameDriveEntity(){}

        public RenameDriveEntity(int serviceId, string id, string newName) : base(serviceId)
        {
            EntityId = id;
            NewName = newName;
        }

        public string EntityId { get; set; }
        public string NewName { get; set; }
    }
}