using Bijector.Infrastructure.Handlers;
using Bijector.Infrastructure.Queues;
using Bijector.Workflows.Handlers;
using Bijector.Workflows.Messages.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Bijector.Workflows
{
    public static class Extensions
    {
        public static ISubscriber SubscribeAllEvenets(this ISubscriber subscriber)
        {            
            return subscriber.
                SubscribeEvent<TimeArrivedEvent>().
                SubscribeEvent<DriveEntityMoved>().
                SubscribeEvent<MoveDriveEntityRejected>().
                SubscribeEvent<DriveEntityRenamed>().
                SubscribeEvent<RenameDriveEntityRejected>().
                SubscribeEvent<ForceStartEvent>();
        }

        public static void RegisterHandlers(this IServiceCollection services)
        {
            services.AddTransient<IEventHandler<TimeArrivedEvent>, TimeArrivedHandler>();
        }
    }
}