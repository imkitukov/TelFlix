using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services.Abstract;
using TelFlix.Services.Contracts;
using TelFlix.Services.Models.Reviews;

namespace TelFlix.Services
{
    public class ReviewService : BaseService, IReviewService
    {
        public ReviewService(TFContext context)
            : base(context)
        {
        }

        public void AddReview(string userId, int movieId, string comment)
        {
            var a = this.Context
                .Reviews               
                .Add(new Review
                {
                    UserId = userId,
                    MovieId = movieId,
                    Comment = comment,
                    CreatedOn = DateTime.Now,
                    Author = Context.Users.Select(x => x).Where(u => u.Id == userId).First()
                });


            this.Context.SaveChanges();
        }

        public IEnumerable<ReviewModel> GetAllByMovieId(int id)
        {
            var movie = this.Context.Movies.FirstOrDefault(m => m.Id == id);

            if (movie != null)
            {
                return this.Context
                            .Reviews
                            .Include(r => r.Author)
                            .Where(r => r.MovieId == id)
                            .OrderByDescending(r => r.CreatedOn)
                            .Select(r => new ReviewModel
                            {
                                Id = r.Id,
                                Author = r.Author.Email,
                                AuthorId = r.Author.Id,
                                CreatedOn = r.CreatedOn,
                                Comment = r.Comment
                            })
                            .ToList();
            }

            return new List<ReviewModel>();
        }
    }
}
