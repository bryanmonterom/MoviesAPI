using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool InTheaters { get; set; }
        public DateTime LaunchDate { get; set; }
        public string Poster { get; set; }
    }
}
