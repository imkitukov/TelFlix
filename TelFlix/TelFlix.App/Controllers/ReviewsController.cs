﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TelFlix.App.Models;
using TelFlix.Data.Models;
using TelFlix.Services.Contracts;
using Microsoft.AspNetCore.Identity;

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
        public IActionResult Add(int id, ReviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                // service method pass comment, movieid, userid
                var userId = this.userManager.GetUserId(User);
                //return RedirectToAction("Details", "Movies", new { id = id });
                reviewService.AddReview(userId, id, model.ToString());
            }

            return NoContent();
        }
    }
}