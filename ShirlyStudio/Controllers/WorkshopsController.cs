using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShirlyStudio.Models;
using WebApplication4.Models;

namespace ShirlyStudio.Controllers
{
    public class WorkshopsController : Controller
    {
        private readonly ShirlyStudioContext _context;

        public WorkshopsController(ShirlyStudioContext context)
        {
            _context = context;
        }

        // GET: Workshops
        public async Task<IActionResult> Index()
        {
            var shirlyStudioContext = _context.Workshop.Include(w => w.Category).Include(w => w.Teacher);
            return View(await shirlyStudioContext.ToListAsync());
        }

        // GET: Workshops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workshop = await _context.Workshop
                .Include(w => w.Category)
                .Include(w => w.Teacher)
                .FirstOrDefaultAsync(m => m.WorkshopId == id);
            if (workshop == null)
            {
                return NotFound();
            }

            return View(workshop);
        }

        // GET: Workshops/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName");
            ViewData["TeacherId"] = new SelectList(_context.Teacher, "TeacherId", "TeacherName");
            return View();
        }

        // POST: Workshops/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkshopId,WorkshopName,CategoryId,FullData,Price,Available_Members,Description,TeacherId,Duration")] Workshop workshop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workshop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", workshop.CategoryId);
            ViewData["TeacherId"] = new SelectList(_context.Teacher, "TeacherId", "TeacherName", workshop.TeacherId);
            return View(workshop);
        }

        // GET: Workshops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workshop = await _context.Workshop.FindAsync(id);
            if (workshop == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", workshop.CategoryId);
            ViewData["TeacherId"] = new SelectList(_context.Teacher, "TeacherId", "TeacherName", workshop.TeacherId);
            return View(workshop);
        }

        // POST: Workshops/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkshopId,WorkshopName,CategoryId,FullData,Price,Available_Members,Description,TeacherId,Duration")] Workshop workshop)
        {
            if (id != workshop.WorkshopId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workshop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkshopExists(workshop.WorkshopId))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", workshop.CategoryId);
            ViewData["TeacherId"] = new SelectList(_context.Teacher, "TeacherId", "TeacherName", workshop.TeacherId);
            return View(workshop);
        }

        // GET: Workshops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workshop = await _context.Workshop
                .Include(w => w.Category)
                .Include(w => w.Teacher)
                .FirstOrDefaultAsync(m => m.WorkshopId == id);
            if (workshop == null)
            {
                return NotFound();
            }

            return View(workshop);
        }

        // POST: Workshops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workshop = await _context.Workshop.FindAsync(id);
            _context.Workshop.Remove(workshop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkshopExists(int id)
        {
            return _context.Workshop.Any(e => e.WorkshopId == id);
        }
        public ActionResult FindAll()
        {
            //var json = _context.Workshop.AsEnumerable().Select(e => new
            //{
            //    id = e.Id,
            //    title = e.Name,
            //    
            //    start = e.FullData
            //}).ToList();
            //// System.Threading.Thread.Sleep(100);
            //return Json(json);
            var sa = new JsonSerializerSettings();
            var md5 = MD5.Create();
            //myColor.R.ToString("X2") + myColor.G.ToString("X2") + myColor.B.ToString("X2");
            if (User.Identity.Name != null)
            { 
              var eventList = from e in _context.Workshop
                            select new
                            {         
                                url = "https://localhost:44336/CustomerRegistrations/confirmation/?WorkshopId=" + e.WorkshopId + "&customermail=" + User.Identity.Name,
                                id = e.WorkshopId,
                                title = e.WorkshopName,
                                description = e.Description,
                                start = e.FullData.ToString(),
                                end = e.FullData.AddHours(e.Duration).ToString(),
                                color = '#' + Color.FromArgb(md5.ComputeHash(Encoding.UTF8.GetBytes(e.Category.CategoryName))[0], md5.ComputeHash(Encoding.UTF8.GetBytes(e.Category.CategoryName))[1], md5.ComputeHash(Encoding.UTF8.GetBytes(e.Category.CategoryName))[2]).R.ToString("X2")
                                + Color.FromArgb(md5.ComputeHash(Encoding.UTF8.GetBytes(e.Category.CategoryName))[0], md5.ComputeHash(Encoding.UTF8.GetBytes(e.Category.CategoryName))[1], md5.ComputeHash(Encoding.UTF8.GetBytes(e.Category.CategoryName))[2]).G.ToString("X2")
                                + Color.FromArgb(md5.ComputeHash(Encoding.UTF8.GetBytes(e.Category.CategoryName))[0], md5.ComputeHash(Encoding.UTF8.GetBytes(e.Category.CategoryName))[1], md5.ComputeHash(Encoding.UTF8.GetBytes(e.Category.CategoryName))[2]).B.ToString("X2")
                            };
                var rows = eventList.ToArray();
                return Json(rows, sa);
            }  
            else
            {
                var eventListWithOutIdentity = from e in _context.Workshop
                                select new
                                {
                                    id = e.WorkshopId,
                                    title = e.WorkshopName,
                                    description = e.Description,
                                    start = e.FullData.ToString(),
                                    end = e.FullData.AddHours(e.Duration).ToString(),
                                    color = '#' + Color.FromArgb(md5.ComputeHash(Encoding.UTF8.GetBytes(e.Category.CategoryName))[0], md5.ComputeHash(Encoding.UTF8.GetBytes(e.Category.CategoryName))[1], md5.ComputeHash(Encoding.UTF8.GetBytes(e.Category.CategoryName))[2]).R.ToString("X2")
                                    + Color.FromArgb(md5.ComputeHash(Encoding.UTF8.GetBytes(e.Category.CategoryName))[0], md5.ComputeHash(Encoding.UTF8.GetBytes(e.Category.CategoryName))[1], md5.ComputeHash(Encoding.UTF8.GetBytes(e.Category.CategoryName))[2]).G.ToString("X2")
                                    + Color.FromArgb(md5.ComputeHash(Encoding.UTF8.GetBytes(e.Category.CategoryName))[0], md5.ComputeHash(Encoding.UTF8.GetBytes(e.Category.CategoryName))[1], md5.ComputeHash(Encoding.UTF8.GetBytes(e.Category.CategoryName))[2]).B.ToString("X2")
                                };
                var rows = eventListWithOutIdentity.ToArray();
                return Json(rows, sa);

            }
            //foreach(var ev in eventList)
            //{
            //    if()
            //}
           
        }

        public async Task<IActionResult> Filter(string WorkshopName, int price, int available_members)
        {

            var shirlyStudioContext = _context.Workshop.Include(w => w.Category).Include(w => w.Teacher);
            // אותה שאילתה לשלושתם רק נדרש להגדיר את המשתנים
            if (WorkshopName == null && price == 0 && available_members == 0) return Json(await shirlyStudioContext.ToListAsync());
            //  if (Name == null) Name = "";
            if (available_members == 0 && price == 0 && WorkshopName != null)
            {

                var m = await (from c in shirlyStudioContext
                                   //לתקן את התנאים - בגללם כל התוצאות יוצאות
                               where (c.WorkshopName.Contains(WorkshopName))
                               orderby c.FullData
                               select c).ToListAsync();
                return Json(m);
            }
            else if (available_members == 0 && price != 0 && WorkshopName == null)
            {
                var q = await (from c in shirlyStudioContext
                                   //לתקן את התנאים - בגללם כל התוצאות יוצאות
                               where (c.Price <= price)
                               orderby c.FullData
                               select c).ToListAsync();
                return Json(q);

            }
            else if (available_members == 0 && price != 0 && WorkshopName != null)
            {
                var q = await (from c in shirlyStudioContext
                                   //לתקן את התנאים - בגללם כל התוצאות יוצאות
                               where (c.Price <= price)
                               where (c.WorkshopName.Contains(WorkshopName))

                               orderby c.FullData
                               select c).ToListAsync();
                return Json(q);

            }
            else if (available_members != 0 && price == 0 && WorkshopName != null)
            {
                var q = await (from c in shirlyStudioContext
                                   //לתקן את התנאים - בגללם כל התוצאות יוצאות
                               where (c.Available_Members >= available_members)
                               where (c.WorkshopName.Contains(WorkshopName))

                               orderby c.FullData
                               select c).ToListAsync();
                return Json(q);
            }
            else if (available_members != 0 && price != 0 && WorkshopName == null)
            {
                var q = await (from c in shirlyStudioContext
                                   //לתקן את התנאים - בגללם כל התוצאות יוצאות
                               where (c.Available_Members >= available_members)
                               where (c.Price <= price)

                               orderby c.FullData
                               select c).ToListAsync();
                return Json(q);

            }

            else if (available_members != 0 && price == 0 && WorkshopName == null)
            {
                var q = await (from c in shirlyStudioContext
                                   //לתקן את התנאים - בגללם כל התוצאות יוצאות
                               where (c.Available_Members >= available_members)

                               orderby c.FullData
                               select c).ToListAsync();
                return Json(q);
            }
            else
            {
                var q = await (from c in shirlyStudioContext
                                   //לתקן את התנאים - בגללם כל התוצאות יוצאות
                               where (c.Available_Members >= available_members)
                               where (c.Price <= price)
                               where (c.WorkshopName.Contains(WorkshopName))
                               orderby c.FullData
                               select c).ToListAsync();
                return Json(q);

            }
        }

                    [HttpGet]
        public JsonResult GetRecordsMonthly()
        {
            return Json(_context.Workshop.OrderBy(n => n.FullData).ToList());
        }
        //for graphs
        [HttpGet]
        public JsonResult Getfreeplace()
        {
            return Json(_context.Workshop.OrderBy(n => n.FullData).ToList());
        }

        //join function
        public JsonResult Tryjoin()
        {
            var list = from Workshop in _context.Workshop
                       join Customer in _context.Customer on Workshop.WorkshopName equals Customer.CustomerName
                       select new { Name = Workshop.WorkshopName, Workshop.Price };
            var result = list.GroupBy(w => w.Name).Select(t => new { id = t.Key, counter = t.Sum(u => u.Price) }).OrderByDescending(c => c.counter).Take(5);
            return Json(result.ToList());
        }
    }
}
