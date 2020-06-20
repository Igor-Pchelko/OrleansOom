using System.Threading.Tasks;
using Orleans;

namespace OrleansOom.Grains
{
    public interface IGrainWithTimer : IGrainWithIntegerKey
    {
        Task ScheduleAsync();

        Task ReportActivated();

        Task ReportDeactivated();
    }
}