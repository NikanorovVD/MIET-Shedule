namespace ServiceLayer.Models
{
    public record TimeDto
    {
        public TimeSpan Start {  get; set; }
        public TimeSpan End { get; set; }
    }
}
