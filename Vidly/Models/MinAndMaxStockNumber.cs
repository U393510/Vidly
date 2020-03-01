using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    /*
     * Below will be use to create custom validate attribute 
     */
    public class MinAndMaxStockNumber: ValidationAttribute
    {
        /*Custom Validation 
        * ValidationCOntext.ObjectInstance will give you access to the containing class 
        * which is Customer in current case. Since it is an object we need to cast it to 
        * Customer. Then we will implement business logic according to which 
        * if user entered Number of stock >1 and <=20 no no validation required so 
        * return validation success else check show validation error. 
        */
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var movie = (Movie)validationContext.ObjectInstance;

            return (movie.NumberInStock == 0 || movie.NumberInStock > 20)
                ? new ValidationResult("The field number in Stock must be between 1 and 20")
                : ValidationResult.Success;
        }
    }
}