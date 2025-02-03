using DataLayer.Entities;
using ServiceLayer.Models.Parser;

namespace ServiceLayer.Services.Parsing
{
    public class MietSheduleAdapterService
    {
        public Couple Adapt(MietCouple mietCouple)
        {
            return new Couple()
            {
                Order = mietCouple.Time.Code,
                Day = mietCouple.Day,
                Name = mietCouple.Class.Name,
                Auditorium = mietCouple.Room.Name,
                Teacher = mietCouple.Class.TeacherFull,
                NormalizedTeacher = mietCouple.Class.TeacherFull.ToUpper(),
                Group = mietCouple.Group.Name,
                WeekType = mietCouple.DayNumber switch
                {
                    0 => WeekType.ПервыйЧислитель,
                    1 => WeekType.ПервыйЗнаменатель,
                    2 => WeekType.ВторойЧислитель,
                    3 => WeekType.ВторойЗнаменатель,
                    _ => throw new InvalidOperationException($"Invalid DayNumber= {mietCouple.DayNumber}")
                }
            };
        }
    }
}
