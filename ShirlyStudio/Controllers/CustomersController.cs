using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShirlyStudio.Models;
using WebApplication4.Models;

namespace ShirlyStudio.Controllers
{
    [Authorize(Roles = "Admin,Customer")]
    public class CustomersController : Controller
    {
        private readonly ShirlyStudioContext _context;

        public CustomersController(ShirlyStudioContext context)
        {
            _context = context;
        }

        // GET: Customers
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customer.ToListAsync());
        }

        // GET: Customers/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.CustomerId == Id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("CustomerId,CustomerName,Age,Email,PhoneNumber")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(Id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,CustomerName,Age,Email,PhoneNumber")] Customer customer)
        {
            if ( id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.CustomerId == Id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            var customer = await _context.Customer.FindAsync(Id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int Id)
        {
            return _context.Customer.Any(e => e.CustomerId == Id);
        }
        public async Task<IActionResult> CreateFromUser(string userJson)
        {
            Customer c = JsonConvert.DeserializeObject<Customer>(userJson);

            if (c != null)
            {
                _context.Add(c);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
    }


}
