using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class Min18YearsIfAMember:ValidationAttribute
    {
        /*Custom Validation 
         * ValidationCOntext.ObjectInstance will give you access to the containing class 
         * which is Customer in current case. Since it is an object we need to cast it to 
         * Customer. Then we will implement business logic according to which 
         * if user selected MembershipType as 1 – PayAsYouGO no validation required so 
         * return validation success else check further birthdate supplied or not if 
         * not tell user birthdate required if this is given then check whether user is 
         * greater than 18 years or not  
         */
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var customer = (Customer)validationContext.ObjectInstance;

            if (customer.MembershipTypeId == MembershipType.Unknown || customer.MembershipTypeId == MembershipType.PayAsYouGo)
                return ValidationResult.Success;
            if (customer.Birthdate == null)
                return new ValidationResult("Birthdate is required.");
            var age = DateTime.Today.Year - customer.Birthdate.Value.Year;
            return (age >= 18) ?
                ValidationResult.Success
                : new ValidationResult("Customer should be atleast 18 years old to go on a membership.");
        }
    }
}