using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;

namespace OrleansOom.Grains
{
    public class GrainWithTimer : Grain, IGrainWithTimer
    {
        private ILogger<GrainWithTimer> _logger;
        private int _counter;
        private readonly IGrainFactory _grainFactory;
        private int _activatedCount;

        public GrainWithTimer(ILogger<GrainWithTimer> logger, IGrainFactory grainFactory)
        {
            _logger = logger;
            _grainFactory = grainFactory;
            _counter = 0;
            _activatedCount = 0;
        }

        public Task ScheduleAsync()
        {
            _logger.LogInformation($"{DateTime.Now:O} Start timer");

            var dueTime = TimeSpan.Zero;
            var period = TimeSpan.FromMilliseconds(500);
            
            RegisterTimer(x => Tick(), null, dueTime, period);
            
            return Task.CompletedTask;
        }

        public Task ReportActivated()
        {
            _activatedCount++;
            return Task.CompletedTask;
        }

        public Task ReportDeactivated()
        {
            _activatedCount--;
            return Task.CompletedTask;
        }

        private async Task Tick()
        {
            _logger.LogInformation($"{DateTime.Now:O} Tick start: {_counter} / {_activatedCount}");

            var grain = _grainFactory.GetGrain<ITestGrain>(_counter);
            await grain.CreateData(1024*1024);
            _counter++;
            
            _logger.LogInformation("Total Memory: {0}", GC.GetTotalMemory(false));
        }
    }
}