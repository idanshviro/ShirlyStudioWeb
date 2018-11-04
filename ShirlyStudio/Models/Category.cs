using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace ShirlyStudio.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "שם קטגוריה")]
        public String CategoryName { get; set; }
        public virtual ICollection<Workshop> Workshops { get; set; }

    }
}
