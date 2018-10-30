using System;
using System.Collections.Generic;
using System.Text;

namespace TelFlix.Services.ViewModels.ActorViewModels
{
    public class ListActorModel
    {
        public string FullName { get; set; }

        public IEnumerable<string> MovieTitles { get; set; }
    }
}
