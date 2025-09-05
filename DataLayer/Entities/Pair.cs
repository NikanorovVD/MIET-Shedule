namespace DataLayer.Entities
{
    public class Pair
    {
        public int Id {  get; set; }
        public int Order { get; set; }
        public int Day { get; set; }
        public string Name { get; set; }
        public string Auditorium { get; set; }
        public WeekType WeekType { get; set; }
        public int TeacherId { get; set; }
        public int GroupId { get; set; }

        public Teacher Teacher { get; set; }
        public Group Group { get; set; }
    }

    public enum WeekType
    {
        ПервыйЧислитель = 0,
        ВторойЧислитель = 1,
        ПервыйЗнаменатель = 2,
        ВторойЗнаменатель = 3
    }
}
