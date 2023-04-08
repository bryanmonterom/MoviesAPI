using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Entities
{
    public class Theater:IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public Point Location { get; set; }
        public List<MoviesTheaters> MoviesTheaters { get; set; }

    }
}
