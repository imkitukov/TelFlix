using System.Collections.Generic;
using TelFlix.Services.ViewModels.MovieViewModels;

namespace TelFlix.Services.Models.Actors
{
    public class ListActorModel
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public IEnumerable<MovieActorListModel> Movies { get; set; }

        public string SmallImageUrl { get; set; }
    }
}
