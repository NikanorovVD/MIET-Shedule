namespace ServiceLayer.Configuration
{
    public class ScheduleSettings
    {
        public DateTime StartDate { get; set; }
        public IEnumerable<TimePairSettings> Times { get; set; }
    }

    public class TimePairSettings
    {
        public int Order { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }
}
