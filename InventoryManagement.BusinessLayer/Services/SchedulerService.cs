using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.BusinessLayer.Services
{
    public class SchedulerService
    {
        private Timer _timer;
        private readonly ILogger<SchedulerService> _logger;

        public SchedulerService(ILogger<SchedulerService> logger)
        {
            _logger = logger;
        }

        public void Start()
        {
            // Schedule the task to run every 5 seconds
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        public void Stop()
        {
            // Stop the timer
            _timer?.Change(Timeout.Infinite, 0);
        }

        private void DoWork(object state)
        {
            // Define the task to be executed
            // This method will be called on each timer tick
            // Add your desired logic here
            _logger.LogInformation("Task executed!");
        }
    }
}
