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
    public class GetPsychologistsController : Controller
    {
        private readonly PsychologistDBContext _context;

        public GetPsychologistsController(PsychologistDBContext context)
        {
            _context = context;
        }

        // GET: GetPsychologists
        public async Task<IActionResult> Index()
        {
            var psychologistDBContext = _context.Psychologist.Include(p => p.Methods_obj).Include(p => p.Specialization_obj);
            return View(await psychologistDBContext.ToListAsync());
        }

        // GET: GetPsychologists/Details/5
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
        public IActionResult Choose()
        {
            ViewData["Psychologist_objId"] = new SelectList(_context.Psychologist, "ID", "Name");
            return View();
        }

        //var result = new GetSessionsController().Create();


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Choose (int Session_ID, DateTime Date_Session, bool Format_Session, int id)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(getSession);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["Psychologist_objId"] = new SelectList(_context.Psychologist, "ID", "Name", getSession.Psychologist_objId);
            // return View(getSession);


            Psychologist psychologist = await _context.Psychologist.FindAsync(id);
           // GetSession Session = await _context.GetSession.Include(t => t.Date_Session==Date_Session).Include(t => t.Format_Session==Format_Session);
            GetSession Session = await _context.GetSession
          .Include(t => t.Date_Session).Include(t => t.Format_Session).Include(t => t.Psychologist_objId)
          .FirstOrDefaultAsync(m => m.Session_ID==Session_ID);

            ViewBag.Psychologist_id = id;
            ViewBag.Date_Session = Date_Session.ToLongDateString();
            ViewBag.Format = Format_Session;


            //return View("ChooseMatch", Matches);
            return View("Confirm",Session);
        }

       // [Bind("Session_ID,Date_Session,Format_Session,Psychologist_objId")]  GetSession getSession

             //var result = new GetSessionsController().Create();


             [HttpPost]
        public ActionResult Confirm(int id, int Session_ID)
        {
            GetSession session = _context.GetSession.Find(Session_ID);
            session.Psychologist_objId = id;       
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
            //return View();
        }


        private bool PsychologistExists(int id)
        {
          return _context.Psychologist.Any(e => e.ID == id);
        }
    }
}
