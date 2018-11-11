using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TelFlix.App.Models;
using TelFlix.Data.Models;
using TelFlix.Services.Contracts;

namespace TelFlix.App.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly IReviewService reviewService;
        private readonly UserManager<User> userManager;


        public ReviewsController(IReviewService reviewService, UserManager<User> userManager)
        {
            this.reviewService = reviewService;
            this.userManager = userManager;
        }

        // GET: Reviews/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int id, string comment)
        {
            if (ModelState.IsValid)
            {
                var userId = this.userManager.GetUserId(User);

                reviewService.AddReview(userId, id, comment);
            }

            return NoContent();
            //return RedirectToAction(nameof(GetMovieReviews), new { id = id });
            //return RedirectToAction("Details", "Movies", new { id = id });
        }

        public IActionResult GetMovieReviews(int id)
        {
            var reviews = this.reviewService.GetAllByMovieId(id);

            return PartialView("_MovieReviewsTable", reviews);
        }
    }
}