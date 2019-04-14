namespace Njoy.Data
{
    public abstract class IPublicIdentity
    {
        /// <summary>
        /// PK which should not be displayed publicly.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Unique index that identifies the entity but might be publicly displayed.
        /// </summary>
        public string Code { get; set; }
    }
}