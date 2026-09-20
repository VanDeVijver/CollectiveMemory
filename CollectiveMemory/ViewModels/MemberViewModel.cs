namespace CollectiveMemory.ViewModels
{
    public class MemberViewModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string FullName => $"{Firstname.Trim()} {Lastname.Trim()}";
        public List<string> Bands { get; set; } = [];
        public List<string> FavoriteMusic { get; set; } = [];
        public List<string> Instruments { get; set; } = [];
        public string Bio { get; set; } = string.Empty;
        public string? Image { get; set; }
        public string InstrumentsDisplay => string.Join(", ", Instruments);
    }
}
