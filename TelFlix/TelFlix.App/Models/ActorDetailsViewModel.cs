using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelFlix.Data.Models;

namespace TelFlix.App.Models
{
    public class ActorDetailsViewModel
    {
        public string FullName { get; set; }

        public int ApiActorId { get; set; }

        public string Biography { get; set; }

        public string PlaceOfBirth { get; set; }
       
        public string ImdbProfileUrl { get; set; }

        public string DateOfBirth { get; set; }
            
        public IEnumerable<Movie> Movies { get; set; }

        public string MediumImageUrl { get; set; }
    }
}
