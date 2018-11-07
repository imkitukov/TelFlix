using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TelFlix.Data.Models.Abstract;

namespace TelFlix.Data.Models
{
    public class Actor : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        public int ApiActorId { get; set; }

        public string Biography { get; set; }

        public string PlaceOfBirth { get; set; }

        public string ImdbId { get; set; }

        public string ImdbProfileUrl { get; set; }

        public string DateOfBirth { get; set; }

        public ICollection<MoviesActors> Movies { get; set; }

        public string SmallImageUrl { get; set; }

        public string MediumImageUrl { get; set; }

        public string LargeImageUrl { get; set; }
    }
}