using MoviesAPI.Validations;

namespace MoviesAPI.DTOs
{
    public class ActorCreationDTO :ActorPatchDTO
    {
     
        [FileSizeValidation(maxSizeInMb:4)]
        [FileTypeValidation(fileTypeGroup: FileTypeGroup.Image)]
        public IFormFile Photo { get; set; }
    }
}
