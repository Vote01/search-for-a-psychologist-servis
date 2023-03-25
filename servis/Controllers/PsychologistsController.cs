﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using servis.Models;

namespace servis.Controllers
{
    public class PsychologistsController : Controller
    {
        private readonly PsychologistDBContext _context;

        public PsychologistsController(PsychologistDBContext context)
        {
            _context = context;
        }

        // GET: Psychologists
        public async Task<IActionResult> Index()
        {
            var psychologistDBContext = _context.Psychologist.Include(p => p.Methods_obj).Include(p => p.Specialization_obj);
            return View(await psychologistDBContext.ToListAsync());
        }

        // GET: Psychologists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Psychologist == null)
            {
                return NotFound();
            }

            var psychologist = await _context.Psychologist
                .Include(p => p.Methods_obj)
                .Include(p => p.Specialization_obj)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (psychologist == null)
            {
                return NotFound();
            }

            return View(psychologist);
        }

        // GET: Psychologists/Create
        public IActionResult Create()
        {
            ViewData["Methods_objId"] = new SelectList(_context.Methods, "Methods_ID", "Methods_Name");
            ViewData["Specialization_objId"] = new SelectList(_context.Specialization, "Special_ID", "Special_Name");
            return View();
        }

        // POST: Psychologists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,LastName,Year,Info,Price,Methods_objId,Specialization_objId")] Psychologist psychologist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(psychologist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Methods_objId"] = new SelectList(_context.Methods, "Methods_ID", "Methods_Name", psychologist.Methods_objId);
            ViewData["Specialization_objId"] = new SelectList(_context.Specialization, "Special_ID", "Special_Name", psychologist.Specialization_objId);
            return View(psychologist);
        }

        // GET: Psychologists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Psychologist == null)
            {
                return NotFound();
            }

            var psychologist = await _context.Psychologist.FindAsync(id);
            if (psychologist == null)
            {
                return NotFound();
            }
            ViewData["Methods_objId"] = new SelectList(_context.Methods, "Methods_ID", "Methods_Name", psychologist.Methods_objId);
            ViewData["Specialization_objId"] = new SelectList(_context.Specialization, "Special_ID", "Special_Name", psychologist.Specialization_objId);
            return View(psychologist);
        }

        // POST: Psychologists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,LastName,Year,Info,Price,Methods_objId,Specialization_objId")] Psychologist psychologist)
        {
            if (id != psychologist.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(psychologist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PsychologistExists(psychologist.ID))
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
            ViewData["Methods_objId"] = new SelectList(_context.Methods, "Methods_ID", "Methods_Name", psychologist.Methods_objId);
            ViewData["Specialization_objId"] = new SelectList(_context.Specialization, "Special_ID", "Special_Name", psychologist.Specialization_objId);
            return View(psychologist);
        }

        // GET: Psychologists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Psychologist == null)
            {
                return NotFound();
            }

            var psychologist = await _context.Psychologist
                .Include(p => p.Methods_obj)
                .Include(p => p.Specialization_obj)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (psychologist == null)
            {
                return NotFound();
            }

            return View(psychologist);
        }

        // POST: Psychologists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Psychologist == null)
            {
                return Problem("Entity set 'PsychologistDBContext.Psychologist'  is null.");
            }
            var psychologist = await _context.Psychologist.FindAsync(id);
            if (psychologist != null)
            {
                _context.Psychologist.Remove(psychologist);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PsychologistExists(int id)
        {
          return _context.Psychologist.Any(e => e.ID == id);
        }
    }
}