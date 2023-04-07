using MoviesAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class ActorCreationDTO
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        [FileSizeValidation(maxSizeInMb:4)]
        [FileTypeValidation(fileTypeGroup: FileTypeGroup.Image)]
        public IFormFile Photo { get; set; }
    }
}
