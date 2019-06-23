using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
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
        
        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist) //In the view, gig.artist.name is null. And that's because we haven't loaded artist.
                .Include(g => g.Genre)
                .ToList();

            /* The model item passed into the dictionary is of type, list of gig,
            but this dictionary requires a model item */
            var viewModel = new GigsViewModel()
            {
                 UpcomingGigs = gigs,
                 ShowActions = User.Identity.IsAuthenticated
            };

            return View(viewModel);
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