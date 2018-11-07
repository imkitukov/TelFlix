using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services.Abstract;
using TelFlix.Services.Contracts;
using TelFlix.Services.Models.Actors;
using TelFlix.Services.Providers.Exceptions;

namespace TelFlix.Services
{
    public class ActorServices : BaseService, IActorServices
    {
        public ActorServices(TFContext context) : base(context)
        {
        }

        public IEnumerable<ListActorModel> ListAllActors()
        {
            var actors = this.Context
                            .Actors
                            .Select(a => new ListActorModel
                            {
                                Id = a.Id,
                                FullName = a.FullName,
                                MovieTitles = a.Movies.Select(m => m.Movie.Title).ToList(),
                                SmallImageUrl = a.SmallImageUrl
                                
                            })
                            // todo add more actor info
                            .ToList();

            return actors;
        }

        public Actor FindActorByName(string fullname)
        {
            var actor = this.Context
                            .Actors
                            .FirstOrDefault(m => m.FullName == fullname);

            //if (actor == null)
            //{
            //    throw new InexistingEntityException(nameof(Actor), firstName + " " + lastName);
            //}

            return actor;
        }
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
    }
}
