using System.Threading.Tasks;
using Bijector.Infrastructure.Handlers;
using Bijector.Infrastructure.Types;
using Bijector.Infrastructure.Types.Messages;
using Bijector.Workflows.Executor;
using Bijector.Workflows.Messages.Events;

namespace Bijector.Workflows.Handlers
{
    public abstract class AllHandler : IEventHandler<IEvent>
    {
        private readonly IWorkflowExecutor executor;

        public AllHandler(IWorkflowExecutor executor)
        {
            this.executor = executor;
        }

        public async Task Handle(IEvent @event, IContext context)
        {
            await executor.Handle(@event, context);
        }
    }

    public class TimeArrivedHandler : AllHandler, IEventHandler<TimeArrivedEvent>
    {
        public TimeArrivedHandler(IWorkflowExecutor executor) : base(executor){}

        public async Task Handle(TimeArrivedEvent command, IContext context)
        {
            await base.Handle(command, context);
        }
    }

    public class DriveEntityRenamedHandler : AllHandler, IEventHandler<DriveEntityRenamed>
    {
        public DriveEntityRenamedHandler(IWorkflowExecutor executor) : base(executor){}

        public async Task Handle(DriveEntityRenamed command, IContext context)
        {
            await base.Handle(command, context);
        }
    }

    public class RenameDriveEntityRejectedHandler : AllHandler, IEventHandler<RenameDriveEntityRejected>
    {
        public RenameDriveEntityRejectedHandler(IWorkflowExecutor executor) : base(executor){}

        public async Task Handle(RenameDriveEntityRejected command, IContext context)
        {
            await base.Handle(command, context);
        }
    }

    public class MoveDriveEntityRejectedHandler : AllHandler, IEventHandler<MoveDriveEntityRejected>
    {
        public MoveDriveEntityRejectedHandler(IWorkflowExecutor executor) : base(executor){}

        public async Task Handle(MoveDriveEntityRejected command, IContext context)
        {
            await base.Handle(command, context);
        }
    }

    public class DriveEntityMovedHandler : AllHandler, IEventHandler<DriveEntityMoved>
    {
        public DriveEntityMovedHandler(IWorkflowExecutor executor) : base(executor){}

        public async Task Handle(DriveEntityMoved command, IContext context)
        {
            await base.Handle(command, context);
        }
    }

    public class ForceStartHandler : AllHandler, IEventHandler<ForceStartEvent>
    {
        public ForceStartHandler(IWorkflowExecutor executor) : base(executor){}

        public async Task Handle(ForceStartEvent command, IContext context)
        {
            await base.Handle(command, context);
        }
    }
}