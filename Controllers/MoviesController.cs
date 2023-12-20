using DVDRental2.Data;
using DVDRental2.Models;
using DVDRental2.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DVDRental2.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;


        // Constructor to inject ApplicationDbContext
        public MoviesController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }


        [HttpGet]
        public async Task<IActionResult> ViewAll()
        {

            // Retrieve all movies from the database
            var movies = await applicationDbContext.Movies.ToListAsync();
            return View(movies);
        }



        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddMovieViewModel addMovieRequest)
        {

            // Create a new movie object from the view model
            var movie = new Moive()
            {

                Id = Guid.NewGuid(),
                Title = addMovieRequest.Title,

                Description = addMovieRequest.Description,

                Director = addMovieRequest.Director,

                Genre = addMovieRequest.Genre,

                ReleaseYear = addMovieRequest.ReleaseYear,

                Star = addMovieRequest.Star,
            };

            // Add the movie to the database and save changes
            await applicationDbContext.AddAsync(movie);
            await applicationDbContext.SaveChangesAsync();
            // Redirect to Add 
            return RedirectToAction("Add");



        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {

            // Find the movie by id in the database
            var movie = await applicationDbContext.Movies.FirstOrDefaultAsync(x => x.Id == id);

            if (movie != null)
            {

                // Create a view model from the movie and render the View page
                var viewModel = new UpdateMovieViewModel()
                {


                    Id = movie.Id,
                    Title = movie.Title,
                    Description = movie.Description,
                    Director = movie.Director,
                    Genre = movie.Genre,
                    ReleaseYear = movie.ReleaseYear,
                    Star = movie.Star,


                };

                return await Task.Run(() => View("View", viewModel));
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> View(UpdateMovieViewModel model)
        {

            // Find the movie by id in the database
            var movie = await applicationDbContext.Movies.FindAsync(model.Id);

            if (movie != null)
            {
                // Update movie properties and save changes
                movie.Title = model.Title;
                movie.Description = model.Description;
                movie.Director = model.Director;
                movie.Genre = model.Genre;
                movie.ReleaseYear = model.ReleaseYear;
                movie.Star = model.Star;

                await applicationDbContext.SaveChangesAsync();

                return RedirectToAction("ViewAll");
            }

            // Redirect to the ViewAll action if movie is not found
            return RedirectToAction("ViewAll");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateMovieViewModel model)
        {


            // Find the movie by id in the database
            var movie = await applicationDbContext.Movies.FindAsync(model.Id);
            if (movie != null)
            {

                // Remove the movie from the database and save changes
                applicationDbContext.Movies.Remove(movie);
                await applicationDbContext.SaveChangesAsync();


                return RedirectToAction("ViewAll");

            }
            // Redirect to the ViewAll action if movie is not found
            return RedirectToAction("ViewAll");
        }
    }
}
