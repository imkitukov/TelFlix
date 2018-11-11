using System.Collections.Generic;
using TelFlix.Data.Models;
using TelFlix.Services.Models.Actors;

namespace TelFlix.Services.Contracts
{
    public interface IActorServices
    {
        IEnumerable<ListActorModel> ListAllActors(int page = 1, int pageSize = 10, string search = "");

        Actor FindActorByName(string fullname);

        ActorDetailModel FindActorById(int id);

        Actor AddActor(Actor actor);

        void AddActorDetails(Actor actor);

        int Count(string search = "");
    }
}
