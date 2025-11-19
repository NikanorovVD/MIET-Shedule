using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities.Virtual
{
    [Keyless]
    public class NearestPair
    {
        public string Name { get; set; }
        public int Order {  get; set; }
        public string Auditorium {  get; set; }
        public int RemainingDays {  get; set; }
        public string TeacherName {  get; set; }
        public DateOnly TargetDate { get; set; }
    }
}
