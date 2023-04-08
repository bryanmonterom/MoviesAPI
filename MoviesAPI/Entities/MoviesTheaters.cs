namespace MoviesAPI.Entities
{
    public class MoviesTheaters
    {
        public int MovieId { get; set; }
        public int TheatherId { get; set; }
        public Movie Movie { get; set; }
        public Theater Theater { get; set; }


    }
}
