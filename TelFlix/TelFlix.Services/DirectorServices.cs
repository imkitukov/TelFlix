//using Microsoft.AspNetCore.Identity;
//using System.Linq;
//using System.Text;
//using TelFlix.Data.Models;
//using TelFlix.Data.UnitOfWorkCore;
//using TelFlix.Services.Abstract;
//using TelFlix.Services.Contracts;
//using TelFlix.Services.Providers.Contracts;

//namespace TelFlix.Services
//{
//    public class DirectorServices : BaseService, IDirectorServices
//    {
//        private const string DirectorAddedSuccessfully = "Director {0} {1} added successfully.";
//        private const string DirectorAlreadyExists = "Director {0} {1} already exists.";

//        private readonly IAssociativeTablesTool associativeTablesTool;

//        public DirectorServices(IUnitOfWork unitOfWork, IAssociativeTablesTool associativeTablesTool, UserManager<User> userManager)
//            : base(unitOfWork, userManager)
//        {
//            this.associativeTablesTool = associativeTablesTool;
//        }

//        public string AddDirector(string[] directors, Movie movie)
//        {
//            StringBuilder sb = new StringBuilder();

//            foreach (var dir in directors)
//            {
//                var splittedNames = dir.Split().ToArray();

//                Director currentDirector = new Director() { FirstName = splittedNames[0] };

//                if (splittedNames.Length > 1)
//                {
//                    currentDirector.LastName = splittedNames.Last();
//                }

//                var existingDirector = this.UnitOfWork.GetRepo<Director>()
//                    .GetFirstOrDefault(d =>
//                        (d.FirstName == currentDirector.FirstName && d.LastName == currentDirector.LastName) ||
//                        (d.FirstName == currentDirector.FirstName && d.LastName == null));

//                if (existingDirector != null)
//                {
//                    sb.AppendLine(string.Format(DirectorAlreadyExists, currentDirector.FirstName, currentDirector.LastName));

//                    currentDirector = existingDirector;
//                }
//                else
//                {
//                    this.UnitOfWork.GetRepo<Director>()
//                        .Add(currentDirector);

//                    this.UnitOfWork.SaveChanges();

//                    sb.AppendLine(string.Format(DirectorAddedSuccessfully, currentDirector.FirstName, currentDirector.LastName));
//                }

//                // adds many to many relation
//                if (movie != null)
//                {
//                    this.associativeTablesTool
//                        .AddAssociativeRelation<MoviesDirectors>(
//                                                     nameof(MoviesDirectors.DirectorId), currentDirector.Id,
//                                                     nameof(MoviesDirectors.MovieId), movie.Id);
//                }
//            }

//            this.UnitOfWork.SaveChanges();

//            return sb.ToString();
//        }
//    }
//}
