using ShirlyStudio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        [Display(Name = "שם מלא")]
        public String CustomerName { get; set; }

        [Display(Name = "גיל")]
        public int Age { get; set; }

        [Required]
        [Display(Name = "כתובת אימייל")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [Required(ErrorMessage = "מספר הטלפון אינו חוקי")]
        [Display(Name = "פלאפון")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "מספר הטלפון אינו חוקי")]
        public String PhoneNumber { get; set; }


        public virtual ICollection<CustomerRegistration> CustomerRegistration { get; set; }


    }
}
