using Quartz;
using ServiceLayer.Services;

namespace MietShedule.Server.QuartzJobs
{
    public class SheduleUpdateJob : IJob
    {
        private readonly SheduleUpdateService _updateService;
        private readonly ILogger<SheduleUpdateJob> _logger;

        public SheduleUpdateJob(SheduleUpdateService updateService, ILogger<SheduleUpdateJob> logger)
        {
            _updateService = updateService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Start shedule update");
            try
            {
                await _updateService.UpdateSheduleAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating shedule: {Error}", ex.ToString());
            }
        }
    }
}
