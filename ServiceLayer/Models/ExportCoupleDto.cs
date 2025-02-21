namespace ServiceLayer.Models
{
    public class ExportCoupleDto
    {
        public int Order { get; set; }
        public int Day { get; set; }
        public string Name { get; set; }
        public string Auditorium { get; set; }
        public string Teacher { get; set; }
        public string Group { get; set; }
        public string WeekType { get; set; }
    }
}
