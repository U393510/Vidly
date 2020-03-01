using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    [Authorize(Roles = RoleName.CanManageMovies)]
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;
        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        /*
         * ApplicationDBContext object is a disposable object 
         * so we should properly dispose it by overriding the dispose 
         * method of the base controller class  
         */
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        [AllowAnonymous]
        // GET: Movies/Random
        public ActionResult Random()
        {
            var movie = new Movie()
            {
                Id = 1,
                Name = "Shrek!"
            };

            
            var customers = new List<Customer>
            {
                new Customer {Name= "Customer 1", Id = 1},
                new Customer {Name = "Customer 2", Id = 2}
            };
            //Let us see demo of viewModel
            var viewModel = new RandomMovieViewModel
            {
                Movie = movie,
                Customers = customers
            };
                

            //Let us see how we can pass data from controller to view using ViewBag
            //by simple choose any variable as property of viewbag as I choosed MyMovie
            ViewBag.MyMovie = movie;
            //To refer movie model property inside view just use <h2>@ViewBag.MyMovie.Name</h2>
            // or <h2>@ViewBag.MyMovie.Id</h2>
            // it is better not to use viewBag and viewdata to transfer instead use the below approach
            //to transfer model to view as it gives compile time safety

            //Will call view name Random as function name is Random by convention
            //We pass the model called movie to view whose properties can be accessed as 
            //@Model.Name in view
            //view() returns a viewResult

           // return View(movie);
            return View(viewModel);
            //return Content("Movie controller");
            //return HttpNotFound();
            //return new EmptyResult();
            //Will redirect to Home controller and Call Index view and pass parameter as query string 
            //parameter as shown http://localhost:3890/?Page=1&SortBy=name
            //return RedirectToAction("Index", "Home", new { Page = 1, SortBy = "name" });
        }
        //In below method we will see how we can make our parameter as nullable 
        // and int parameter nullable is represented by int? and for string ? not required
        //as string is passed by ref and is already nullable 
        //example http://localhost:3890/movies?pageIndex=1&sortBy=tom
        //Get Movies
        public ActionResult Index1(int? pageIndex, string sortBy)
        {
            if (!pageIndex.HasValue)
            {
                pageIndex = 10;
            }
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                sortBy = "Name";
            }
            //return Content("PageIndex = " + pageIndex + " Sort By = " + sortBy);
            return Content(string.Format("PageIndex ={0}&Sort By ={1}", pageIndex, sortBy));
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            //Let us eagerly load Movies and Genre so 
            //var movies = _context.Movies.Include(m => m.Genre).ToList();
            //return View(movies);
            if (User.IsInRole(RoleName.CanManageMovies))
                return View("Indexdt");
            return View("ReadOnlyList");
        }
        //Index data table will generate a data table using Movie web api
        [AllowAnonymous]
        public ActionResult Indexdt()
        {
            return View();
        }
        public ActionResult NewMovie()
        {
            var MovieFormViewModel = new MovieFormViewModel
            {
                //since we have set the empty movie which basically set the default values
                //which we don't want to show therefore we will modify it below line use 
                //an other code
                //Movie = new Movie(), //Replace this line with below lines 
                Genres = _context.Genres.ToList()
            };
            return View("MovieForm", MovieFormViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie)
        {
            /*
             * When ASP.NET MVC populate this Customer object passed as parameter with request data it checks to 
             * see if the object is valid based on the data annotations applied on various 
             * properties of this customer class so at this point we can use Model state 
             * property to get access to Validation data. It has a property called IsValid 
             * which we can use to change the application flow
             */
            if (!ModelState.IsValid)
            {
                //Model state is invalid so render the same view again to user inorder enable re-entry
                var viewModel = new MovieFormViewModel(movie)
                {
                    Genres = _context.Genres.ToList()
                };
                return View("MovieForm", viewModel);

            }
            //Check whether a new movie or an existing movie
            if (movie.Id==0)
            {
                //In case New Movide then add it to DB
                _context.Movies.Add(movie);
            }
            else
            {
                //Existing Movie and Update desired properties 
                var MovieInDb = _context.Movies.Single(m => m.Id == movie.Id);
                MovieInDb.Name = movie.Name;
                MovieInDb.ReleaseDate = movie.ReleaseDate;
                MovieInDb.DateAdded = movie.DateAdded;
                MovieInDb.NumberInStock = movie.NumberInStock;
                MovieInDb.GenreId = movie.GenreId;

                //In current case we want to update all the properties of Movie 
                //so we can use TryUpdateModel method to update existing movie with
                //Values inside Request Data i.e. entered by user
                //TryUpdateModel(MovieInDb);
            }
            _context.SaveChanges();
            return RedirectToAction ("Index","Movies");
        }
        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);
            //Check whether Dbcontext returned a movie with given Id or not
            if (movie == null)
            {
                return HttpNotFound();
            }
            var viewModel = new MovieFormViewModel(movie)
            {

                Genres = _context.Genres.ToList()
            };
            return View("MovieForm", viewModel);
        }
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            var movie = _context.Movies.Include(m => m.Genre).SingleOrDefault(m => m.Id == id);
            return View(movie);
        }
        /*
         * Below is the example of URL with constraints defined within Route attribute
         * using regular expression where \d{4} means year must entered of 4 digits
         * Month must be entered of 2 digits and month range should be 1 to 12
         * Please note no spaces should be given between like {Year:regex()}
         * Other constraints that can be defined are min, max, minlength, maxlength,
         * int, float, guid. If you want to know How these constrains can be applied  
         * please google "ASP.NET MVC Attribute Route Constraints"
         */
        [Route("movies/released/{year:regex(\\d{4})}/{month:regex(\\d{2}):range(1, 12)}")]
        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content(string.Format("Year = {0} & MovieMonth = {1}", year,month));
        }
    }
}