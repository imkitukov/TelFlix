using System;

namespace TelFlix.Services.ViewModels.MovieViewModels
{
    public class MovieActorListModel
    {
        public MovieActorListModel(int id, string title)
        {
            this.Id = id;
            this.Title = title ?? throw new ArgumentNullException(nameof(title));
        }

        public int Id { get; set; }

        public string Title { get; set; }
    }
}
