using MoviesAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class MovieCreationDTO :MoviePatchDTO
    {
   
        [FileSizeValidation(maxSizeInMb:4)]
        [FileTypeValidation(FileTypeGroup.Image)] 
        public IFormFile Poster { get; set; }
    }
}
