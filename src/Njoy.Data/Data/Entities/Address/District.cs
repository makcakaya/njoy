namespace Njoy.Data
{
    public sealed class District : EntityBase
    {
        public string Name { get; set; }
        public County County { get; set; }
        public int CountyId { get; set; }
    }
}