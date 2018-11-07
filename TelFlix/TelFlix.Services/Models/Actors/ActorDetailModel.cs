using System.Collections.Generic;

namespace TelFlix.Services.Models.Actors
{
    public class ActorDetailModel
    {
        public string FullName { get; set; }

        public int ApiActorId { get; set; }

        public string Biography { get; set; }

        public string PlaceOfBirth { get; set; }

        public string ImdbProfileUrl { get; set; }

        public string DateOfBirth { get; set; }

        public IEnumerable<Data.Models.Movie> Movies { get; set; }

        public string MediumImageUrl { get; set; }
    }
}
