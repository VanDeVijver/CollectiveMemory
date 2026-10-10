namespace CollectiveMemory.Core.Entities
{
    /// <summary>
    /// A day the band can't rehearse. The whole band shares one login, so a day is simply free or
    /// not: if one member can't, nobody can. The note says who or why.
    /// </summary>
    public class UnavailableDay : BaseEntity
    {
        public DateOnly Date { get; set; }
        public string? Note { get; set; }
    }
}
