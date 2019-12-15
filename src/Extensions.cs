using Bijector.Infrastructure.Queues;

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