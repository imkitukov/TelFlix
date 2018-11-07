using System;
using System.Collections.Generic;
using System.Text;

namespace TelFlix.Services.Models.Actors
{
    public class ListActorModel
    {
        public int Id { get; set; }

        public string FullName { get; set; }
        
        public IEnumerable<string> MovieTitles { get; set; }

        public int ApiActorId { get; set; }

        public string ImdbId { get; set; }

        public string ImdbProfileUrl { get; set; }

        public DateTime? DateOfBirth { get; set; }
        
        public string SmallImageUrl { get; set; }

        public string MediumImageUrl { get; set; }

        public string LargeImageUrl { get; set; }
    }
}
