using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShirlyStudio.Models
{
    public class ClusterResulter
    {
        [Key]
        [Required] public int ClusterResulterID { get; set; }
        [Required] public int WokshopId { get; set; }
        [Required] public int ClusterRes { get; set; }

    }
}
