using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data.Entity;
using System.Net.Http;
using System.Web.Http;
using Vidly.Models;
using Vidly.Dtos;
using AutoMapper;

namespace Vidly.Controllers.Api
{
    public class MoviesController : ApiController
    {
        private ApplicationDbContext _context;
        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }
        //Get api/movies
        //Passing optional query parameter which help in filterring relevant records
        public IHttpActionResult GetMovies(string query = null)
        {
            //Here we will use link extension method called Select and pass delegate 
            //which does the mapping between Movie and Movie DTO
            //Now we want to show Genre along with Movie name in our view 
            //so we need to eagerly load Genre along with Movie entity 
            //but our MovieDto doesn't have genre so we need to provide it in movieDTO
            //return Ok(_context.Movies.Include(m=>m.Genre).ToList().Select(Mapper.Map<Movie, MovieDto>));
            //Below will give us movieQuery variable which will IQueryable interface 
            var moviesQuery = _context.Movies.Include(m => m.Genre).Where(m=>m.NumberAvailable > 0);
            //Apply filter condition on movieQuery
            if (!string.IsNullOrWhiteSpace(query))
            {
                moviesQuery = moviesQuery.Where(m => m.Name.Contains(query));
            }
            //Run the query and get movies 
            var movieDto = moviesQuery
                .ToList()
                .Select(Mapper.Map<Movie, MovieDto>);
            return Ok(movieDto);
        }

        //Get api/movies/1
        public IHttpActionResult GetMovie(int id)
        {
            var movie = _context.Movies.SingleOrDefault(c => c.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<Movie, MovieDto>(movie));
        }
        //Post : Create Movie and return httpstatus code 201
        [HttpPost]
        public IHttpActionResult CreateMovie(MovieDto movieDto)
        {
            //check the Movie Model state of the passed in
            if (!ModelState.IsValid)
            {
                //if not valid simply return Bad request
                return BadRequest();
            }
            //else convert movieDto to movie type 
            var movie = Mapper.Map<MovieDto, Movie>(movieDto);
            //Insert it in to Database
            _context.Movies.Add(movie);
            _context.SaveChanges();
            //Finally set the newly inserted movie Id to movieDto
            movieDto.Id = movie.Id;
            /*
             * In restful convention when we create a resource status code 
             * by convention should be returned as 201 instead of  
             * 200 – OK.So we need create Movie WebApi as per below 
             * Suppose new create movied id is 10 then Uri would be /api/Movie/10
             */
            return Created(new Uri(Request.RequestUri + "/" + movie.Id), movieDto);
        }
        //Put - Update Movie api/movie/1 
        [HttpPut]
        public void UpdateMovie(int id, MovieDto movieDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var movieInDB = _context.Movies.SingleOrDefault(m => m.Id == id);
            if (movieInDB == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            //Map movieDto properties to movie in Database 
            //to update with new data in dto and save the changes in DB
            /*Note : Following Exception will generate during execution of below line called 
             * Property ‘Id’ is part of object’s key information and cannot be modified.
             * To resolve this, you need to tell AutoMapper to ignore Id during mapping 
             * of a MovieDto to Movie. So just add in MappingProfile file following code 
             * Mapper.CreateMap<MovieDto, Movie>().ForMember(m=>m.Id,opt => opt.Ignore()
             */
            Mapper.Map(movieDto, movieInDB);
            _context.SaveChanges();
        }
        //Delete - Movie
        [HttpDelete]
        public void DeleteMovie(int id)
        {
            var movieInDB = _context.Movies.SingleOrDefault(m => m.Id == id);
            if (movieInDB == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            _context.Movies.Remove(movieInDB);
            _context.SaveChanges();
        }
    }
}
