using AutoMapper;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<GenreCreationDTO, Genre>();


            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreationDTO, Actor>().ForMember(a => a.Photo, options => options.Ignore());
            CreateMap<ActorPatchDTO, Actor>().ReverseMap();


            CreateMap<Movie, MovieDTO>().ReverseMap();
            CreateMap<MovieCreationDTO, Movie>()
                .ForMember(a => a.Poster, options => options.Ignore())
                .ForMember(a => a.MoviesGenres, options => options.MapFrom(MapMoviesGenres))
                .ForMember(a=> a.MoviesActors, options => options.MapFrom(MapMoviesActors));


            CreateMap<MoviePatchDTO, Movie>().ReverseMap();
        }

        private List<MoviesGenres> MapMoviesGenres(MovieCreationDTO movieCreationDTO, Movie movie)
        {

            var result = new List<MoviesGenres>();

            if (movieCreationDTO.GenresId == null)
            {
                return result;
            }

            foreach (var id in movieCreationDTO.GenresId)
            {
                result.Add(new MoviesGenres { GenreId = id });
            }

            return result;
        }

        private List<MoviesActors> MapMoviesActors(MovieCreationDTO movieCreationDTO, Movie movie) {
            var result = new List<MoviesActors>();

            if (movieCreationDTO.Actors == null)
            {
                return result;
            }

            foreach (var actor in movieCreationDTO.Actors)
            {
                result.Add(new MoviesActors {  ActorId = actor.ActorId, Character = actor.Character  });
            }

            return result;
        }

    }
}
