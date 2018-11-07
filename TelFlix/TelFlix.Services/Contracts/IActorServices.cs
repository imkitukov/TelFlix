using System.Collections.Generic;
using TelFlix.Data.Models;
using TelFlix.Services.Models.Actors;

namespace TelFlix.Services.Contracts
{
    public interface IActorServices
    {
        IEnumerable<ListActorModel> ListAllActors();

        Actor FindActorByName(string fullname);

        Actor FindActorById(int id);

        //void EditActor(Actor actor, string columnToChange, string newValue);

        Actor AddActor(Actor actor);
    }
}
