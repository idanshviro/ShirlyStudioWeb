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

        // GET: CustomerRegistrations
        public async Task<IActionResult> Index()
        {
            var shirlyStudioContext = _context.CustomerRegistration.Include(c => c.Customer).Include(c => c.Workshop);
            return View(await shirlyStudioContext.ToListAsync());
        }
        public async Task<IActionResult> MyIndex()
        {
            var Customer = from c in _context.CustomerRegistration.Include(c => c.Customer).Include(c => c.Workshop)
                           where (c.Customer.Email.Equals(User.Identity.Name))
                           select c;
            //var shirlyStudioContext = _context.CustomerRegistration.Include(c => c.Customer).Include(c => c.Workshop);
            return View(await Customer.ToListAsync());
        }


        // GET: CustomerRegistrations/Details/5
        public async Task<IActionResult> Details(int? CustomerRegistrationId)
        {
            if (CustomerRegistrationId == null)
            {
                return NotFound();
            }

            var customerRegistration = await _context.CustomerRegistration
                .Include(c => c.Customer)
                .Include(c => c.Workshop)
                .FirstOrDefaultAsync(m => m.CustomerRegistrationId == CustomerRegistrationId);
            if (customerRegistration == null)
            {
                return NotFound();
            }

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
                    await _context.SaveChangesAsync();
                    //return RedirectToAction(nameof(HomeController.Index));
                    return RedirectToAction("MyIndex", "CustomerRegistration");
                }
                //return RedirectToAction(nameof(HomeController.Index));
                // return RedirectToAction("Index", "Home");
               return RedirectToAction("MyIndex", "CustomerRegistration");
            }
            else
            {
                ViewData["error"] = "לקוח יקר, הנך רשום כבר לסדנה זו";
                return RedirectToAction("Error", "Home");
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
