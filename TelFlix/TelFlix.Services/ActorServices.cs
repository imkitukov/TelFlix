using System.Collections.Generic;
using System.Linq;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services.Abstract;
using TelFlix.Services.Contracts;
using TelFlix.Services.Providers.Exceptions;
using TelFlix.Services.ViewModels.ActorViewModels;

namespace TelFlix.Services
{
    public class ActorServices : BaseService, IActorServices
    {
        public ActorServices(TFContext context) : base(context)
        {
        }

        public ICollection<ListActorModel> ListAllActors()
        {
            var actors = this.Context
                            .Actors
                            .Select(a => new ListActorModel
                            {
                                FullName = a.FullName,
                                MovieTitles = a.Movies.Select(m => m.Movie.Title).ToList()
                            })
                            // todo add more actor info
                            .ToList();

            return actors;
        }

        //public void EditActor(Actor actor, string columnToEdit, string newValue)
        //{
        //    if (columnToEdit == FirstNameColumn)
        //    {
        //        actor.FirstName = newValue;
        //    }

        //    if (columnToEdit == LastNameColumn)
        //    {
        //        actor.LastName = newValue;
        //    }

        //    if (columnToEdit == BirthdayColumn)
        //    {
        //        //06-12-1999
        //        var isValid = DateTime.TryParseExact(newValue, "d-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result);

        //        if (!isValid)
        //        {
        //            throw new InvalidDateFormatException();
        //        }

        //        actor.DateOfBirth = result;
        //    }

        //    this.UnitOfWork.SaveChanges();
        //}

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
    }
}
