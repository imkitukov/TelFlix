using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using TelFlix.App.Areas.Admin.Models;
using TelFlix.Data.Models;

namespace TelFlix.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "RequireAdministratorRole")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        // GET: User
        public IActionResult Index()
        {
            var users = this.userManager
                .Users
                .Select(u => new ListUserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email
                })
                .ToList();

            return View(users);
        }

        public async Task<IActionResult> Roles(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await this.userManager.GetRolesAsync(user);

            return View(new UserWithRolesViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Roles = roles
            });
        }

        // GET: User/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create() => View();

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                await this.userManager.CreateAsync(new Data.Models.User
                {
                    Email = model.Email,
                    UserName = model.Email,
                }, model.Password);

                var user = await this.userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    return NotFound();
                }

                await this.userManager.AddToRoleAsync(user, "RegularUser");
                this.TempData["SuccessMessage"] = $"User {user.Email} created successfully.";

                return RedirectToAction(nameof(Index));
            }
        }

        // GET: User/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit()
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(new DeleteUserViewModel
            {
                Id = id,
                Email = user.Email
            });
        }

        public async Task<ActionResult> Destroy(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            await this.userManager.DeleteAsync(user);

            this.TempData["SuccessMessage"] = $"User {user.Email} deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddToRole(string id)
        {
            var rolesSelectListItems = this.roleManager
                .Roles
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
                .ToList();

            if (rolesSelectListItems.Count == 0)
            {
                return NotFound();
            }

            return View(rolesSelectListItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToRole(string id, string role)
        {
            var user = await this.userManager.FindByIdAsync(id);
            var roleExists = await this.roleManager.RoleExistsAsync(role);

            if (user == null || !roleExists)
            {
                return NotFound();
            }

            await this.userManager.AddToRoleAsync(user, role);

            TempData["SuccessMessage"] = $"User {user.Email} added successfully to {role} role.";
            return RedirectToAction(nameof(Index));
        }
    }
}