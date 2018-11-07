using System.Collections.Generic;
using TelFlix.Data.Models;
using TelFlix.Services.ViewModels.ActorViewModels;

namespace TelFlix.Services.Contracts
{
    public interface IActorServices
    {
        ICollection<ListActorModel> ListAllActors();

        Actor FindActorByName(string fullname);

        //void EditActor(Actor actor, string columnToChange, string newValue);

        Actor AddActor(Actor actor);

        void AddActorDetails(Actor actor);
    }
}
