﻿using System;
using System.Collections.Generic;
using System.Text;
using TelFlix.Data.Models;

namespace TelFlix.Services.Models.Movie
{
    public class MovieDetailModel
    {
        public int Id { get; set; }

        public int ApiMovieId { get; set; }

        public string Title { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int? DurationInMinutes { get; set; }

        public IEnumerable<Actor> Actors { get; set; }

        public IEnumerable<Genre> Genres { get; set; }
        
        public IEnumerable<Review> Reviews { get; set; }
        
        public float? Rating { get; set; }

        public string Description { get; set; }

        public string SmallImageUrl { get; set; }

        public string MediumImageUrl { get; set; }

        public string TrailerUrl { get; set; }
    }
}
