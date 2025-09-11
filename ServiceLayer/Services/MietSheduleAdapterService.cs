using DataLayer.Entities;
using MietClient;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.Services
{
    public class MietSheduleAdapterService
    {
        public IEnumerable<Pair> AdaptShedule(IEnumerable<MietPair> mietPairs)
        {
            Dictionary<string, Teacher> teachers = new(
                mietPairs.Select(
                    p => new Teacher()
                    {
                        Name = p.Class.TeacherFull.Trim(),
                    })
                .Distinct(new TeacherNameComparer())
                .ToDictionary(t => t.Name));

            Dictionary<string, Group> groups = new(
                mietPairs.Select(
                    p => new Group()
                    {
                        Name = p.Group.Name.Trim(),
                    })
                .Distinct(new GroupNameComparer())
                .ToDictionary(g => g.Name));

            return mietPairs.Select(p => Adapt(p, teachers, groups));
        }

        private Pair Adapt(MietPair mietPair, Dictionary<string, Teacher> teachers, Dictionary<string, Group> groups)
        {
            return new Pair()
            {
                Order = mietPair.Time.Code,
                Day = mietPair.Day,
                Name = mietPair.Class.Name,
                Auditorium = mietPair.Room.Name,
                Teacher = teachers[mietPair.Class.TeacherFull.Trim()],
                Group = groups[mietPair.Group.Name.Trim()],
                WeekType = mietPair.DayNumber switch
                {
                    0 => WeekType.ПервыйЧислитель,
                    1 => WeekType.ПервыйЗнаменатель,
                    2 => WeekType.ВторойЧислитель,
                    3 => WeekType.ВторойЗнаменатель,
                    _ => throw new InvalidOperationException($"Invalid DayNumber: {mietPair.DayNumber}")
                }
            };
        }
    }

    class TeacherNameComparer : IEqualityComparer<Teacher>
    {
        public bool Equals(Teacher? x, Teacher? y)
        {
            if ((x is null) || (y is null)) return false;
            return x.Name.Equals(y.Name);
        }

        public int GetHashCode([DisallowNull] Teacher obj)
        {
            return obj.Name.GetHashCode();
        }
    }

    class GroupNameComparer : IEqualityComparer<Group>
    {
        public bool Equals(Group? x, Group? y)
        {
            if ((x is null) || (y is null)) return false;
            return x.Name.Equals(y.Name);
        }

        public int GetHashCode([DisallowNull] Group obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
