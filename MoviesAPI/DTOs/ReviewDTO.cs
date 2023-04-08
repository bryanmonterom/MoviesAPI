﻿using Microsoft.AspNetCore.Identity;
using MoviesAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Rate { get; set; }
        public int MovieId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

    }
}
