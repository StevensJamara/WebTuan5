using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BT.Service
{
    public class LifetimeManager : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;
        private const int LifetimeInMilliseconds = 14 * 24 * 60 * 60 * 1000; // 2 weeks

        public LifetimeManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ResetService, null, LifetimeInMilliseconds, Timeout.Infinite);
            return Task.CompletedTask;
        }

        private void ResetService(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var bookService = scope.ServiceProvider.GetRequiredService<IBookService>() as BookService;
                // Perform reset logic here, e.g., clearing or reinitializing the book collection
                bookService?.ClearBooks();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
