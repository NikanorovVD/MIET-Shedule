namespace DataLayer.Entities
{
    public enum WeekType
    {
        ПервыйЧислитель,
        ВторойЧислитель,
        ПервыйЗнаменатель,
        ВторойЗнаменатель
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
