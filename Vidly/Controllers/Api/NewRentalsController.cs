using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class NewRentalsController : ApiController
    {
        ApplicationDbContext _context;
        public NewRentalsController()
        {
            _context = new ApplicationDbContext();
        }
        //Input here is Customre ID and Movies 
        [HttpPost]
        public IHttpActionResult CreateNewRentals(NewRentalDto newRental )
        {
            //Check whethe any Movie passed in or not of not passed 
            //we don't want to make any call to DB so placed on top
            if (newRental.MovieIds.Count() == 0)
            {
                return BadRequest("No movie ids have been given.");
            }

            //We have not used SingleOrDefault because we belive customer id will be provided correctly 
            //if not then exception will be thrown to malicious user 
            var customer = _context.Customers.SingleOrDefault
                (c => c.Id == newRental.CustomerId);
            if (customer == null)
            {
                return BadRequest("Invalid Customer ID.");
            }

            //Below we constructed a query SELECT* FROM MOVIES WHERE Id IN(1,2,3)
            //using LINQ
            var movies = _context.Movies.Where(
                m => newRental.MovieIds.Contains(m.Id)).ToList();
            //Check whether movie ids passed in are correct  or not 
            if (movies.Count() != newRental.MovieIds.Count())
            {
                return BadRequest("One or more movie Ids are invalid");
            }
            foreach (var movie in movies)
            {
                if (movie.NumberAvailable == 0)
                {
                    return BadRequest("Movie is not available.");
                }
                movie.NumberAvailable--;

                var rental = new Rental
                {
                    Customer = customer,
                    Movie = movie,
                    DateRented = DateTime.Now
                };
                _context.Rentals.Add(rental);
            }
            _context.SaveChanges();
            //Note here we have not called created method as we are not creating single object.
            // so we can not pass back URL/ObjectId so just returning OK.
            return Ok();

        }

    }
}
