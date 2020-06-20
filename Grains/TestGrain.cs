using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;

namespace OrleansOom.Grains
{
    public class TestGrain : Grain, ITestGrain
    {
        private readonly ILogger<TestGrain> _logger;
        private readonly IGrainFactory _grainFactory;
        private List<byte[]> _dataList;

        public TestGrain(ILogger<TestGrain> logger, IGrainFactory grainFactory)
        {
            _logger = logger;
            _grainFactory = grainFactory;
            _dataList = new List<byte[]>();
        }

        public Task<bool> CreateData(long size)
        {
            _logger.LogInformation($"Allocate: {size}");

            var data = CreateBuffer(size);
            _dataList.Add(data);
            
            return Task.FromResult(true);
        }
        
        static byte[] CreateBuffer(long size)
        {
            var buffer = new byte[size];
            for (int j = 0; j < buffer.Length; j++)
            {
                buffer[j] = 1;
            }
            return buffer;
        }
        
        public override Task OnActivateAsync()
        {
            var grain = _grainFactory.GetGrain<IGrainWithTimer>(0);
            grain.ReportActivated();
            return Task.CompletedTask;
        }

        /// <summary>
        /// This method is called at the beginning of the process of deactivating a grain.
        /// </summary>
        public override Task OnDeactivateAsync()
        {
            var grain = _grainFactory.GetGrain<IGrainWithTimer>(0);
            grain.ReportDeactivated();
            return Task.CompletedTask;
        }
    }
}