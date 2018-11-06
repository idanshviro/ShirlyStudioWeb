using ShirlyStudio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{

    public class Workshop
    {
        [Key]
        public int WorkshopId { get; set; }

        [Required]
        [Display(Name = "שם סדנה")]
        public String WorkshopName { get; set; }

        [Required]
         [Display(Name = "קטגוריה")]
        public int CategoryId { get; set; }
        [Display(Name = "קטגוריה")]
        public virtual Category Category { get; set; }


        [Required]
        [Display(Name = "תאריך הסדנה")]
        public DateTime FullData { get; set; }

        [Required]
        [Display(Name = "מחיר הסדנה")]
        public int Price { get; set; }

        [Required]
        [Display(Name = "מספר מקומות פנויים")]
        public int Available_Members { get; set; }

        [Display(Name = "פרטים נוספים")]
        public String Description { get; set; }

        [Required]
        [Display(Name = "שם המורה")]
        public int TeacherId { get; set; }

        [Display(Name = "שם המורה")]
        public virtual Teacher Teacher { get; set; }

        [Display(Name = "משך הסדנה")]
        public double Duration { get; set; }

        [Display(Name = "רישומים")]
        public virtual ICollection<CustomerRegistration> CustomerRegistrations { get; set; } }

    }

