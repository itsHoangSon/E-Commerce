using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.BackgroundTasks
{
    public interface IBackgroundMediatRQueue
    {
        Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken);
        Task Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken);
    }
    public class BackgroundMediatRQueue : IBackgroundMediatRQueue
    {
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public BackgroundMediatRQueue(IBackgroundTaskQueue backgroundTaskQueue, IServiceScopeFactory serviceScopeFactory)
        {
            _backgroundTaskQueue = backgroundTaskQueue;
            _serviceScopeFactory = serviceScopeFactory;
        }




        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken)
        {
            if (notification != null)
            {
                await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(async token =>
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        await mediator.Publish(notification, cancellationToken);
                    }
                });
            }
        }


        public async Task Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
        {
            await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(async token =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Send(request, cancellationToken);
                }
            });
        }
    }
}
