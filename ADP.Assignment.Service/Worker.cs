using ADP.Assignment.Common.Extensions;
using ADP.Assignment.Domain.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ADP.Assignment.Service
{
    public class Worker : BackgroundService
    {
        private readonly IMathService _mathService;
        private readonly ILogger<Worker> _logger;

        public Worker(IMathService mathService, ILogger<Worker> logger)
        {
            _logger = logger;
            _mathService = mathService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _mathService.CalculateInstructionAsync();
                    await Task.Delay(1000, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.GetErrorMsg());
                }                
            }
        }
    }
}