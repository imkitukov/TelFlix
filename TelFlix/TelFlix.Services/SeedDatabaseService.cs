//using Microsoft.AspNetCore.Identity;
//using System;
//using System.IO;
//using System.Threading.Tasks;
//using TelFlix.Data.Contracts;
//using TelFlix.Data.Models;
//using TelFlix.Data.Models.Contracts;
//using TelFlix.Data.UnitOfWorkCore;
//using TelFlix.Services.Abstract;
//using TelFlix.Services.Contracts;

//namespace TelFlix.Services
//{
//    public class SeedDatabaseService : ISeedDatabaseService
//    {
//        private readonly IMovieRepository movieRepo;
//        private readonly IAddMovieService addMovieService;
//        private const string SeedData = "../TelFlix.Services/Resources/movie-titles.txt";

//        public SeedDatabaseService(IMovieRepository movieRepository, IAddMovieService addMovieService)
//        {
//            this.movieRepo = movieRepository;
//            this.addMovieService = addMovieService;
//        }

//        public bool Check<T>() where T : class, IDeletable
//        {
//            return this.movieRepo.IsSeeded();
//        }

//        public void SeedAsync()
//        {
//            var movieTitles = File
//                .ReadAllText(SeedData)
//                .Split(Environment.NewLine);

//            //await Task.Run(() =>
//            //{
//                foreach (var title in movieTitles)
//                {
//                    this.addMovieService.AddMovie(title);
//                }
//            //    ;
//            //});
//        }
//    }
//}
