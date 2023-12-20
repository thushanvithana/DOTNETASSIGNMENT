namespace DVDRental2.Models
{
    public class UpdateMovieViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Star { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseYear { get; set; }
    }
}
