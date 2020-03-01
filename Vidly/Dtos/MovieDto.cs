using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Dtos
{
    public class MovieDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [Required]
        public short NumberInStock { get; set; }

        [Required]
        public byte GenreId { get; set; }

        //Here we have not used Genre directly as 
        //we don't tight coupling btween Movie Dto and Genre so 
        //we create GenreDto separately and defined in Mapping profile also
        //so we can perform Movie to MovieDto mapping easily 
        public GenreDto Genre { get; set; }
    }
}