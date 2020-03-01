using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vidly.Models;

namespace Vidly.ViewModels
{
    public class MovieFormViewModel
    {
        public IEnumerable<Genre> Genres { get; set; }
        //since we have set the empty movie which basically set the default values
        //which we don't want to show therefore we will modify it below line use 
        //an other code
        // public Movie Movie { get; set; } Replace this line with below lines and make 
        //properties nullable for our viewModel form as this will not populate form 
        // with default values
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d MMM yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        [Display(Name = "Release Date")]
        public DateTime? ReleaseDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d MMM yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        [Display(Name = "Date Added")]
        public DateTime? DateAdded { get; set; }

        [Required]
        [MinAndMaxStockNumber]
        [Display(Name = "Number in Stock")]
        public short? NumberInStock { get; set; }

        [Required]
        public byte GenreId { get; set; }

        //Form title property to the name of the form dynamically
        public string Title
        {
            get {
                    return Id != 0 ?  "Edit Movie":  "New Movie";
                }
        }
        //Will create new movie
        public MovieFormViewModel()
        {
            //initialize Id to zero so that it gets populated in 
            //MovieForm.cshtml hidden field called @Html.HiddenFor(m=>m.Id)
            Id = 0;
        }
        public MovieFormViewModel(Movie movie)
        {
            Id = movie.Id;
            Name = movie.Name;
            ReleaseDate = movie.ReleaseDate;
            NumberInStock = movie.NumberInStock;
            GenreId = movie.GenreId;
            DateAdded = movie.DateAdded;
        }
        
    }
}