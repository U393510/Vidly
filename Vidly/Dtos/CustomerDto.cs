using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vidly.Models;

namespace Vidly.Dtos
{
    public class CustomerDto
    {
         public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        //Since our Min18YearsIfAMember validator is using customer object 
        //we will not be able to use it with customerDto 
        //[Min18YearsIfAMember]
        public DateTime? Birthdate { get; set; }
        public bool IsSubscribedToNewsletter { get; set; }
        //Here we have not used MembershipType directly as 
        //we don't tight coupling btween Dto and MembershipType so 
        //we create MembershipTypeDto separately and defined in Mapping profile
        public MembershipTypeDto MembershipType { get; set; }
        [Required]
        public byte MembershipTypeId { get; set; }
    }
}