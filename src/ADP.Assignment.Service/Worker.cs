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
                    var result = await _mathService.CalculateInstructionAsync();
                    if (result.IsSuccess) _logger.LogInformation(result.ToJson());                    
                    else _logger.LogError(result.ToJson());
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, ex.GetErrorMsg());
                }
                finally
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}