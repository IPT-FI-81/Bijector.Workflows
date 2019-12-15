using Bijector.Infrastructure.Queues;
using Bijector.Workflows.Messages.Events;

namespace Bijector.Workflows
{
    public static class Extensions
    {
        public static ISubscriber SubscribeAllEvenets(this ISubscriber subscriber)
        {
            return subscriber;
        }
    }
}