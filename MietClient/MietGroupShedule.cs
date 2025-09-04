namespace MietClient
{
    public class MietGroupShedule
    {
        public IEnumerable<MietTime> Times { get; set; }
        public IEnumerable<MietPair> Data { get; set; }
        public string Semestr { get; set; }
    }

    public class MietPair
    {
        public int Day { get; set; }
        public int DayNumber { get; set; }
        public MietTime Time { get; set; }
        public MietClass Class { get; set; }
        public MietGroup Group { get; set; }
        public MietRoom Room { get; set; }
    }

    public class MietTime
    {
        public int Code { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
    }

    public class MietClass
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string TeacherFull { get; set; }
        public string Teacher { get; set; }
    }

    public class MietGroup
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class MietRoom
    {
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
