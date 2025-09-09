namespace ServiceLayer.Models
{
    public class TeacherPairGroupedDto
    {
        public DateOnly Date { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Teacher { get; set; }
        public TimeDto Time { get; set; }
        public IEnumerable <string> Auditoriums { get; set; }
        public IEnumerable <string> Groups { get; set; }
    }
}
