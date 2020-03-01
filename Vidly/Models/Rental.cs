using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class Rental
    {
        public int Id { get; set; }

        //Navigation property
        [Required]
        public Movie Movie { get; set; }

        //Navigation property
        [Required]
        public Customer Customer { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d MMM yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name ="Date Rented")]
        public DateTime DateRented { get; set; }

        //Nulllable DateReturned 
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d MMM yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name ="Date Returned")]
        public DateTime? DateReturned { get; set; }


    }
}