namespace ServiceLayer.Models
{
    public class PairDto
    {
        public DateOnly Date { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Auditorium { get; set; }
        public string Teacher { get; set; }
        public string Group { get; set; }
        public TimeDto Time {  get; set; }
    }
}
