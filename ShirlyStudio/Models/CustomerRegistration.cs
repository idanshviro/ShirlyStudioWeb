using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace ShirlyStudio.Models
{
    public class CustomerRegistration
    {
        public int CustomerRegistrationId { get; set; }
        public int WorkshopId { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Workshop Workshop { get; set; }
    }
}
