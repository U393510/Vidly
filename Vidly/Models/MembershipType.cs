using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class MembershipType
    {

        //Since we have very small number of Memberships so we use Id
        public byte Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        //Since signup fee is not going to have value > 32000 so we use short type
        public short SignUpFee { get; set; }
        //For duration in months we use byte type as we are not going to store value > 12
        public byte DurationInMonths { get; set; }
        //for DiscountRate also we are not going to store more than 100 so use Byte
        public byte DiscountRate { get; set; }
        /*Instead of using hard coded numbers such as 1 or 0 in our code to 
          indentity membership type we can simply declare static readonly properties 
          so that we may not modify them accidently lateron in our code 
          We can use Enums also but in that case we  need to typecast it in comparison
          so therefore prefer to use static properties
        */
        public static readonly byte Unknown = 0;
        public static readonly byte PayAsYouGo = 1;
    }
}