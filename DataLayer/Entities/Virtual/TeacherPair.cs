using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities.Virtual
{
    [Keyless]
    public class TeacherPair
    {
        public DateOnly Date { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Auditorium { get; set; }
        public string Teacher { get; set; }
        public string Group { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }
}
