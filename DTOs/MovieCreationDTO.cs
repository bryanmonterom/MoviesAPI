using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Helpers;
using MoviesAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class MovieCreationDTO : MoviePatchDTO
    {

        [FileSizeValidation(maxSizeInMb: 4)]
        [FileTypeValidation(FileTypeGroup.Image)]
        public IFormFile Poster { get; set; }
        [ModelBinder(binderType: typeof(TypeBinder<List<int>>))]
        public List<int> GenresId { get; set; }
        [ModelBinder(binderType: typeof(TypeBinder<List<ActorMovieCreationDTO>>))]
        public List<ActorMovieCreationDTO> Actors { get; set; }

    }
}
