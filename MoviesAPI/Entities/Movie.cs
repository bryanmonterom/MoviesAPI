﻿using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Entities
{
    public class Movie : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public bool InTheaters { get; set; }
        public DateTime LaunchDate { get; set; }
        public string Poster { get; set; }
        public List<MoviesActors> MoviesActors { get; set; }
        public List<MoviesGenres> MoviesGenres { get; set; }
        public List<MoviesTheaters> MoviesTheaters { get; set; }


    }
}
