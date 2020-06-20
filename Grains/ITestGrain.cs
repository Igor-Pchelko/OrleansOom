using System.Threading.Tasks;
using Orleans;

namespace OrleansOom.Grains
{
    public interface ITestGrain : IGrainWithIntegerKey
    {
        Task<bool> CreateData(long size);
    }
}