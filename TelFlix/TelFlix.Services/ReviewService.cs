using System;
using System.Collections.Generic;
using System.Text;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services.Abstract;
using TelFlix.Services.Contracts;

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
            this.Context
                .Reviews
                .Add(new Review
                {
                    UserId = userId,
                    MovieId = movieId,
                    Comment = comment,
                    CreatedOn = DateTime.Now
                });


            this.Context.SaveChanges();
        }


    }
}
