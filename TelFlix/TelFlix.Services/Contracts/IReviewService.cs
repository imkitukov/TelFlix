using System.Collections.Generic;
using TelFlix.Services.Models.Reviews;

namespace TelFlix.Services.Contracts
{
    public interface IReviewService
    {
        void AddReview(string userId, int movieId, string comment);

        IEnumerable<ReviewModel> GetAllByMovieId(int id);

        IEnumerable<ReviewModel> GetAllByUserId(string userId);
    }
}
