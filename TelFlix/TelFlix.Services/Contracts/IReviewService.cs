using System;
using System.Collections.Generic;
using System.Text;
using TelFlix.Data.Models;

namespace TelFlix.Services.Contracts
{
    public interface IReviewService
    {
        void AddReview(string userId, int movieId, string comment);
    }
}
