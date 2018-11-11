using System.Collections.Generic;
using System.Linq;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services.Abstract;
using TelFlix.Services.Contracts;
using TelFlix.Services.Models.Actors;
using TelFlix.Services.Providers.Exceptions;
using TelFlix.Services.ViewModels.MovieViewModels;

namespace TelFlix.Services
{
    public class ActorServices : BaseService, IActorServices
    {
        public ActorServices(TFContext context) : base(context)
        {
        }

        public IEnumerable<ListActorModel> ListAllActors(int page = 1, int pageSize = 10, string search = "")
        {
            var actors = this.Context.Actors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                actors = actors.Where(a => a.FullName.ToLower().Contains(search.ToLower()));
            }

            return actors
                        .OrderBy(a => a.FullName)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .Select(a => new ListActorModel
                        {
                            Id = a.Id,
                            FullName = a.FullName,
                            Movies = a.Movies.Select(m => new MovieActorListModel(m.Movie.Id, m.Movie.Title)).ToList(),
                            SmallImageUrl = a.SmallImageUrl
                        })
                        .ToList();
        }

        public Actor FindActorByName(string fullname)
                 => this.Context
                        .Actors
                        .FirstOrDefault(m => m.FullName == fullname);

        public ActorDetailModel FindActorById(int id)
        {
            var actor = this.Context
                .Actors
                .Where(m => m.Id == id)
                .Select(a => new ActorDetailModel
                {
                    FullName = a.FullName,
                    MediumImageUrl = a.MediumImageUrl,
                    ApiActorId = a.ApiActorId,
                    Biography = a.Biography,
                    DateOfBirth = a.DateOfBirth,
                    ImdbProfileUrl = a.ImdbProfileUrl,
                    PlaceOfBirth = a.PlaceOfBirth,
                    Movies = a.Movies.Select(m => m.Movie).ToList()
                })
                .FirstOrDefault();

            return actor;
        }

        public Actor AddActor(Actor actor)
        {
            var existingActor = this.Context
                                 .Actors
                                 .FirstOrDefault(a => a.ApiActorId == actor.ApiActorId);

            if (existingActor != null)
            {
                if (existingActor.IsDeleted == false)
                {
                    throw new EntityAlreadyExistingException(nameof(Actor), existingActor.FullName);
                }

                // restore the actor
                existingActor.IsDeleted = false;
            }
            else
            {
                // Add actor
                this.Context.Actors.Add(actor);
            }

            this.Context.SaveChanges();

            return actor;
        }

        public void AddActorDetails(Actor actorDto)
        {
            var existingActor = this.Context.Actors.FirstOrDefault(a => a.Id == actorDto.Id);

            if (existingActor != null)
            {
                existingActor.DateOfBirth = actorDto.DateOfBirth;
                existingActor.Biography = actorDto.Biography;
                existingActor.ImdbProfileUrl = actorDto.ImdbProfileUrl;
                existingActor.ImdbId = actorDto.ImdbId;
                existingActor.PlaceOfBirth = actorDto.PlaceOfBirth;

                this.Context.SaveChanges();
            }
        }

        public int Count(string search = "")
        {
            var actors = this.Context.Actors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                actors = actors.Where(a => a.FullName.ToLower().Contains(search.ToLower()));
            }

            return actors.Count();
        }
    }
}
