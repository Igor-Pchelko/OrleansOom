using System.Threading;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime;

namespace OrleansOom.Grains
{
    public class GrainWithTimerStartup : IStartupTask
    {
        private readonly IGrainFactory _grainFactory;

        public GrainWithTimerStartup(IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }

        public Task Workload()
        {
            throw new System.NotImplementedException();
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            var cancelStaleReservations = _grainFactory.GetGrain<IGrainWithTimer>(0);
            await cancelStaleReservations.ScheduleAsync();        
        }
    }
}