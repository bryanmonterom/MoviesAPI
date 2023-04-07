using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class TheaterCreationDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
    }
}
