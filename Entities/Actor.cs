using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Entities
{
    public class Actor :IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public  DateTime DOB { get; set; }
        public string Photo { get; set; }
        public List<MoviesActors> MoviesActors { get; set; }


    }
}
