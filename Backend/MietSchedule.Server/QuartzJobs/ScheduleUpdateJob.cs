using Quartz;
using ServiceLayer.Services;

namespace MietSchedule.Server.QuartzJobs
{
    public class ScheduleUpdateJob : IJob
    {
        private readonly ScheduleUpdateService _updateService;
        private readonly ILogger<ScheduleUpdateJob> _logger;

        public ScheduleUpdateJob(ScheduleUpdateService updateService, ILogger<ScheduleUpdateJob> logger)
        {
            _updateService = updateService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Start schedule update");
            try
            {
                await _updateService.UpdateScheduleAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating schedule: {Error}", ex.ToString());
            }
        }
    }
}
