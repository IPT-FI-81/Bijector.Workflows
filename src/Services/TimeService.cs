using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Bijector.Infrastructure.Queues;
using Bijector.Workflows.Messages.Events;
using Bijector.Infrastructure.Types;

namespace Bijector.Workflows.Services
{
    public class TimeService : IHostedService, IDisposable
    {
        private Timer timer;
        
        private readonly IPublisher publisher;

        public TimeService(IPublisher publisher)
        {
            this.publisher = publisher;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(SendEvent, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
            return Task.CompletedTask;
        }

        private void SendEvent(object state)
        {
            var @event = new TimeArrivedEvent
            {
                Time = DateTimeOffset.Now
            };

            var context = new BaseContext(-1,-1,"Bijector Workflows", "Bijector Workflows");

            publisher.Publish(@event, context).GetAwaiter().GetResult();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }        
}