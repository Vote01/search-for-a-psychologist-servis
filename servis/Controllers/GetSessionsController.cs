using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using servis.Models;

namespace servis.Controllers
{
    public class GetSessionsController : Controller
    {
        private readonly PsychologistDBContext _context;

        public GetSessionsController(PsychologistDBContext context)
        {
            _context = context;
        }

        public GetSessionsController()
        {
        }

        // GET: GetSessions
        public async Task<IActionResult> Index()
        {
            var psychologistDBContext = _context.GetSession.Include(g => g.Psychologist_obj);
            return View(await psychologistDBContext.ToListAsync());
        }

        // GET: GetSessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GetSession == null)
            {
                return NotFound();
            }

            var getSession = await _context.GetSession
                .Include(g => g.Psychologist_obj)
                .FirstOrDefaultAsync(m => m.Session_ID == id);
            if (getSession == null)
            {
                return NotFound();
            }

            return View(getSession);
        }

        // GET: GetSessions/Create
        public IActionResult Create()
        {
            ViewData["Psychologist_objId"] = new SelectList(_context.Psychologist, "ID", "Name");
            return View();
        }

        // POST: GetSessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Session_ID,Date_Session,Format_Session,Psychologist_objId")] GetSession getSession)
        {
            if (ModelState.IsValid)
            {
                _context.Add(getSession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Psychologist_objId"] = new SelectList(_context.Psychologist, "ID", "Name", getSession.Psychologist_objId);
            return View(getSession);
        }

        // GET: GetSessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GetSession == null)
            {
                return NotFound();
            }

            var getSession = await _context.GetSession.FindAsync(id);
            if (getSession == null)
            {
                return NotFound();
            }
            ViewData["Psychologist_objId"] = new SelectList(_context.Psychologist, "ID", "Name", getSession.Psychologist_objId);
            return View(getSession);
        }

        // POST: GetSessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Session_ID,Date_Session,Format_Session,Psychologist_objId")] GetSession getSession)
        {
            if (id != getSession.Session_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(getSession);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GetSessionExists(getSession.Session_ID))
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
            ViewData["Psychologist_objId"] = new SelectList(_context.Psychologist, "ID", "Name", getSession.Psychologist_objId);
            return View(getSession);
        }

        // GET: GetSessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GetSession == null)
            {
                return NotFound();
            }

            var getSession = await _context.GetSession
                .Include(g => g.Psychologist_obj)
                .FirstOrDefaultAsync(m => m.Session_ID == id);
            if (getSession == null)
            {
                return NotFound();
            }

            return View(getSession);
        }

        // POST: GetSessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GetSession == null)
            {
                return Problem("Entity set 'PsychologistDBContext.GetSession'  is null.");
            }
            var getSession = await _context.GetSession.FindAsync(id);
            if (getSession != null)
            {
                _context.GetSession.Remove(getSession);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GetSessionExists(int id)
        {
          return _context.GetSession.Any(e => e.Session_ID == id);
        }
    }
}
