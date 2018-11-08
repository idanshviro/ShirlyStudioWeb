using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShirlyStudio.Models;

namespace ShirlyStudio.Controllers
{
    public class CustomerRegistrationsController : Controller
    {
        private readonly ShirlyStudioContext _context;
        public CustomerRegistrationsController(ShirlyStudioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Filterregistration(string teacher, string wname,string cname)
        {

            var shirlyStudioContext = _context.Workshop.Include(w => w.Category).Include(w => w.Teacher).Include(w=>w.CustomerRegistrations) ;
            // אותה שאילתה לשלושתם רק נדרש להגדיר את המשתנים
            if (teacher == null && wname == null && cname == null) return Json(await shirlyStudioContext.ToListAsync());
            //  if (Name == null) Name = "";
            if (cname == null && wname == null && teacher != null)
            {

                var m = await (from c in shirlyStudioContext
                                   //לתקן את התנאים - בגללם כל התוצאות יוצאות
                               where (c.Teacher.TeacherName.Contains(teacher))
                               orderby c.FullData
                               select c).ToListAsync();
                return Json(m);
            }
            else if (cname == null && wname != null && teacher == null)
            {
                var q = await (from c in shirlyStudioContext
                                   //לתקן את התנאים - בגללם כל התוצאות יוצאות
                               where (c.WorkshopName.Contains(wname))
                               orderby c.FullData
                               select c).ToListAsync();
                return Json(q);

            }
            else if (cname ==null && wname != null && teacher != null)
            {
                var q = await (from c in shirlyStudioContext
                                   //לתקן את התנאים - בגללם כל התוצאות יוצאות
                               where (c.WorkshopName.Contains(wname))
                               where (c.Teacher.TeacherName.Contains(teacher))

                               orderby c.FullData
                               select c).ToListAsync();
                return Json(q);

            }
            else if (cname != null && wname == null && teacher != null)
            {
                var q = await (from w in _context.Workshop
                               join cr in _context.CustomerRegistration

                               on w.WorkshopName equals cr.Workshop.WorkshopName

                               //לתקן את התנאים - בגללם כל התוצאות יוצאות
                               where (cr.Customer.CustomerName.Contains(cname)) 
                               where (w.Teacher.TeacherName.Contains(teacher))

                               orderby w.FullData
                               select cr).ToListAsync();
                return Json(q);
            }
            else if (cname != null && wname != null && teacher == null)
            {
                var q = await (from w in _context.Workshop
                               join cr in _context.CustomerRegistration

                               on w.WorkshopName equals cr.Workshop.WorkshopName

                               //לתקן את התנאים - בגללם כל התוצאות יוצאות
                               where (cr.Customer.CustomerName.Contains(cname))
                               where (w.WorkshopName.Contains(wname))

                               orderby w.FullData
                               select cr).ToListAsync();
                return Json(q);

            }

            else if (cname != null && wname == null && teacher == null)
            {
                var q = await (from c in _context.Customer
                                   //לתקן את התנאים - בגללם כל התוצאות יוצאות
                               where (c.CustomerName.Contains(cname))

                               orderby c.CustomerName
                               select c).ToListAsync();
                return Json(q);
            }
            else
            {
                // var q = await (from c in shirlyStudioContext
                //לתקן את התנאים - בגללם כל התוצאות יוצאות
                var q = await (from w in _context.Workshop
                               join cr in _context.CustomerRegistration

                               on w.WorkshopName equals cr.Workshop.WorkshopName

                               //לתקן את התנאים - בגללם כל התוצאות יוצאות
                               where (cr.Customer.CustomerName.Contains(cname))
                               where (w.WorkshopName.Contains(wname))
                               where (w.Teacher.TeacherName.Contains(teacher))

                               orderby w.FullData
                               select cr).ToListAsync(); 
                return Json(q);

            }
        }
        

        // GET: CustomerRegistrations
        public async Task<IActionResult> Index()
        {
            var shirlyStudioContext = _context.CustomerRegistration.Include(c => c.Customer).Include(c => c.Workshop).Include(c => c.Workshop.Teacher).Include(c => c.Workshop.Category);
            return View(await shirlyStudioContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> MyIndex()
        {
            var Customer = from c in _context.CustomerRegistration.Include(c => c.Customer).Include(c => c.Workshop).Include(c => c.Workshop.Teacher).Include(c => c.Workshop.Category)
                           where (c.Customer.Email.Equals(User.Identity.Name))
                           select c;
            //var shirlyStudioContext = _context.CustomerRegistration.Include(c => c.Customer).Include(c => c.Workshop);
            return View(await Customer.ToListAsync());
        }


        // GET: CustomerRegistrations/Details/5
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var customerRegistration = await _context.CustomerRegistration
                .Include(c => c.Customer)
                .Include(c => c.Workshop).Include(c=>c.Workshop.Teacher).Include(c => c.Workshop.Category)
                .FirstOrDefaultAsync(m => m.CustomerRegistrationId == Id);

            if (customerRegistration == null)
            {
                return NotFound();
            }
            ViewData["WorkshopName"] = customerRegistration.Workshop.WorkshopName;
            ViewData["CustomerName"] = customerRegistration.Customer.CustomerName;
            ViewData["Time"] = customerRegistration.Workshop.FullData.Day + "/" + customerRegistration.Workshop.FullData.Month + "/" + customerRegistration.Workshop.FullData.Year 
                + "  " + customerRegistration.Workshop.FullData.TimeOfDay + "-" 
                + customerRegistration.Workshop.FullData.AddHours(customerRegistration.Workshop.Duration).TimeOfDay;
           ViewData["Teacher"] = customerRegistration.Workshop.Teacher.TeacherName;
            ViewData["Price"] = customerRegistration.Workshop.Price;
            ViewData["Category"] = customerRegistration.Workshop.Category.CategoryName;
            return View(customerRegistration);
        }

        public async Task<IActionResult> MyDetails(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var customerRegistration = await _context.CustomerRegistration
                .Include(c => c.Customer)
                .Include(c => c.Workshop).Include(c => c.Workshop.Teacher).Include(c => c.Workshop.Category)
                .FirstOrDefaultAsync(m => m.CustomerRegistrationId == Id);

            if (customerRegistration == null)
            {
                return NotFound();
            }
            ViewData["WorkshopName"] = customerRegistration.Workshop.WorkshopName;
            ViewData["CustomerName"] = customerRegistration.Customer.CustomerName;
            ViewData["Time"] = customerRegistration.Workshop.FullData.Day + "/" + customerRegistration.Workshop.FullData.Month + "/" + customerRegistration.Workshop.FullData.Year
                + "  " + customerRegistration.Workshop.FullData.TimeOfDay + "-"
                + customerRegistration.Workshop.FullData.AddHours(customerRegistration.Workshop.Duration).TimeOfDay;
            ViewData["Teacher"] = customerRegistration.Workshop.Teacher.TeacherName;
            ViewData["Price"] = customerRegistration.Workshop.Price;
            ViewData["Category"] = customerRegistration.Workshop.Category.CategoryName;
            return View(customerRegistration);
        }
        // GET: CustomerRegistrations/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Email");
            ViewData["WorkshopId"] = new SelectList(_context.Workshop, "WorkshopId", "WorkshopName");
            return View();
        }

        public  IActionResult Confirmation(int workshopid, string customermail)
        {

            if ((workshopid == null) || (customermail == null))
            {
                return NotFound();
            }

            
            var Workshop = from w in _context.Workshop.Include(w => w.Category).Include(w => w.Teacher)
                           where (w.WorkshopId.Equals(workshopid))
                          select w;

            var Customer = from c in _context.Customer
                           where (c.Email.Equals(customermail))
                           select c;

            // return Json(m);
            ViewData["WorkshopName"] = Workshop.First().WorkshopName;
            ViewData["CustomerId"] = Customer.First().CustomerId;
            ViewData["CustomerName"] = Customer.First().CustomerName;
            ViewData["WorkshopId"] = workshopid;
            ViewData["Time"] = Workshop.First().FullData.Day+"/"+ Workshop.First().FullData.Month+"/"+ Workshop.First().FullData.Year + "  " + Workshop.First().FullData.TimeOfDay + "-" + Workshop.First().FullData.AddHours(Workshop.First().Duration).TimeOfDay;
            ViewData["Teacher"] = Workshop.First().Teacher.TeacherName;
            ViewData["Price"] = Workshop.First().Price;
            ViewData["Category"] = Workshop.First().Category.CategoryName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirmation([Bind("CustomerRegistrationId,WorkshopId,CustomerId")] CustomerRegistration customerRegistration)
        {
            var Registration = from R in _context.CustomerRegistration
                           where ((R.WorkshopId.Equals(customerRegistration.WorkshopId))&(R.CustomerId.Equals(customerRegistration.CustomerId)))
                           select R;
            if  (!Registration.Any())
            {
                if (ModelState.IsValid)
                {
                   

                    _context.Add(customerRegistration);
                    var Workshop = from R in _context.Workshop.Include(w => w.Category).Include(w => w.Teacher)
                                   where (R.WorkshopId.Equals(customerRegistration.WorkshopId))
                                       select R;
                        Workshop.First().Available_Members = Workshop.First().Available_Members - 1;
                      _context.Workshop.Update(Workshop.First());
                    //_context.Workshop. Entry(Workshop).State = EntityState.Modified;
                    await _context.SaveChangesAsync();


                    //return RedirectToAction(nameof(HomeController.Index));
                    // return RedirectToAction("MyIndex","CustomerRegistration");
                    return RedirectToAction(nameof(MyIndex));
                }
                //return RedirectToAction(nameof(HomeController.Index));
                 return RedirectToAction("Index","Home");
             // return RedirectToAction("MyIndex", "CustomerRegistration");
            }
            else
            {
                return RedirectToAction("Error","Home", new { message = "!לקוח יקר, הנך כבר רשום לסדנה זו" });
            }
        }

        // POST: CustomerRegistrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerRegistrationId,WorkshopId,CustomerId")] CustomerRegistration customerRegistration)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerRegistration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Email", customerRegistration.CustomerId);
            ViewData["WorkshopId"] = new SelectList(_context.Workshop, "WorkshopId", "WorkshopName", customerRegistration.WorkshopId);
            return View(customerRegistration);
        }

        // GET: CustomerRegistrations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerRegistration = await _context.CustomerRegistration.FindAsync(id);
            if (customerRegistration == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Email", customerRegistration.CustomerId);
            ViewData["WorkshopId"] = new SelectList(_context.Workshop, "WorkshopId", "WorkshopName", customerRegistration.WorkshopId);
            return View(customerRegistration);
        }

        // POST: CustomerRegistrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerRegistrationId,WorkshopId,CustomerId")] CustomerRegistration customerRegistration)
        {
            if (id != customerRegistration.CustomerRegistrationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerRegistration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerRegistrationExists(customerRegistration.CustomerRegistrationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Email", customerRegistration.CustomerId);
            ViewData["WorkshopId"] = new SelectList(_context.Workshop, "WorkshopId", "WorkshopName", customerRegistration.WorkshopId);
            return View(customerRegistration);
        }

        // GET: CustomerRegistrations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerRegistration = await _context.CustomerRegistration
                .Include(c => c.Customer)
                .Include(c => c.Workshop)
                .FirstOrDefaultAsync(m => m.CustomerRegistrationId == id);
            if (customerRegistration == null)
            {
                return NotFound();
            }

            return View(customerRegistration);
        }

        // POST: CustomerRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerRegistration = await _context.CustomerRegistration.FindAsync(id);
            var Workshop = from R in _context.Workshop.Include(w => w.Category).Include(w => w.Teacher)
                           where (R.WorkshopId.Equals(customerRegistration.WorkshopId))
                           select R;
            Workshop.First().Available_Members = Workshop.First().Available_Members + 1;
            _context.Workshop.Update(Workshop.First());
            _context.CustomerRegistration.Remove(customerRegistration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerRegistrationExists(int id)
        {
            return _context.CustomerRegistration.Any(e => e.CustomerRegistrationId == id);
        }

    
        }


}
