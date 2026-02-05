namespace MietClient
{
    public class MietGroupSchedule
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
        private int _code;
        public int Code
        {
            get => _code; set
            {
                if (value > 8 || value < 1)
                {
                    throw new ArgumentException($"Invalid pair order: {value}");
                }
                _code = value;
            }
        }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
    }

    public class MietClass
    {
        public string Name { get; set; }
        public string TeacherFull { get; set; }
        public string Teacher { get; set; }
    }

    public class MietGroup
    {
        public string Name { get; set; }
    }

    public class MietRoom
    {
        public string Name { get; set; }
    }
}
