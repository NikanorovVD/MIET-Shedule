namespace ServiceLayer.Models
{
    public class GrouppedCoupleDto
    {
        public DateTime Date { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Teacher { get; set; }
        public IEnumerable <string> Auditorium { get; set; }
        public IEnumerable <string> Group { get; set; }
        public string Time { get; set; }
    }
}
