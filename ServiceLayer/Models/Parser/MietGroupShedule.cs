namespace ServiceLayer.Models.Parser
{
    public class MietGroupShedule
    {
        public IEnumerable<Time> Times { get; set; }
        public IEnumerable<MietCouple> Data { get; set; }
        public string Semestr { get; set; }
    }

    public class MietCouple
    {
        public int Day { get; set; }
        public int DayNumber { get; set; }
        public Time Time { get; set; }
        public Class Class { get; set; }
        public Group Group { get; set; }
        public Room Room { get; set; }
    }
    public class Time
    {
        public int Code { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
    }

    public class Class
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string TeacherFull { get; set; }
        public string Teacher { get; set; }
    }

    public class Group
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class Room
    {
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
