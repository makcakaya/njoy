namespace Njoy.Data
{
    public sealed class County : EntityBase
    {
        public string Name { get; set; }
        public City City { get; set; }
        public int CityId { get; set; }
    }
}