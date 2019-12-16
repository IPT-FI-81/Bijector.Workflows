using Bijector.Infrastructure.Handlers;
using Bijector.Infrastructure.Queues;
using Bijector.Workflows.Handlers;
using Bijector.Workflows.Messages.Events;
using Bijector.Workflows.Models;
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
            services.AddTransient<IEventHandler<DriveEntityRenamed>, DriveEntityRenamedHandler>();
            services.AddTransient<IEventHandler<DriveEntityMoved>, DriveEntityMovedHandler>();
            services.AddTransient<IEventHandler<RenameDriveEntityRejected>, RenameDriveEntityRejectedHandler>();
            services.AddTransient<IEventHandler<MoveDriveEntityRejected>, MoveDriveEntityRejectedHandler>();
            services.AddTransient<IEventHandler<ForceStartEvent>, ForceStartHandler>();
        }

        public static void RegisterBSONTypes(this IServiceCollection services)
        {
            MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<TimeStartWorkflowNode>();
            MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<CommandWorkflowNode>();
            MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<Messages.Commands.RenameDriveEntity>();
            MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<WorkflowNodeTimeStamps>();
        }
    }
}