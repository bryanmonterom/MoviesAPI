namespace MoviesAPI.DTOs
{
    public class MovieDetailsDTO:MovieDTO
    {
        public List<GenreDTO> Genres { get; set; }
        public List<MovieActorDetailDTO> Actors { get; set; }

    }
}
