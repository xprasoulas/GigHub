using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        //create a database context

        private readonly ApplicationDbContext _context;

        // Initialize the _context

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Gigs
        [Authorize]
        public ActionResult Create()
        {
            //get the list of Genres of database

            var viewModel = new GigFormViewModel
            {
                Genres = _context.Genres.ToList()
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateA(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("Create", viewModel);
            }
            //convert ViewModel to a GigObject, added to our context and save changes
            //Get User Id from Database
            //var artist = _context.Users.Single(u => u.Id == ArtistId);
            //var genre = _context.Genres.Single(g => g.Id == viewModel.Genre );

            var gig = new Gig()
            { 
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            //Add to our context and Save
            _context.Gigs.Add(gig);
            _context.SaveChanges();
            return RedirectToAction("Index","Home");
        }
    }
}