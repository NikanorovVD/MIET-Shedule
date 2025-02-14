namespace DataLayer.Entities
{
    public enum WeekType
    {
        ПервыйЧислитель = 0,
        ВторойЧислитель = 1,
        ПервыйЗнаменатель = 2,
        ВторойЗнаменатель = 3
    }

    public class Couple
    {
        public int Id {  get; set; }
        public int Order { get; set; }
        public int Day { get; set; }
        public string Name { get; set; }
        public string Auditorium { get; set; }
        public string Teacher { get; set; }
        public string NormalizedTeacher { get; set; }
        public string Group { get; set; }
        public WeekType WeekType { get; set; }
    }
}
