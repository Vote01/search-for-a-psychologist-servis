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
            ViewBag.Psychologist_id = id;
            return View(psychologist);
           // return View("Choose");
        }


        public async Task<IActionResult> Client(int? id, int? idC)
        {      
            var psychologistDBContext = _context.Client;
            ViewBag.Psychologist_id = id;
            ViewBag.ClientID = idC;
            return View(await psychologistDBContext.ToListAsync());
        }

        public async Task<IActionResult> ClientDetails(int? id, int? idC)
        {

            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.ID == idC);
            if (client == null)
            {
                return NotFound();
            }
            ViewBag.Psychologist_id = id;
            ViewBag.ClientID = idC;

            return View(client);
          
        }


        [HttpPost]
        public async Task<IActionResult> Choose(int id, int idC)
        {
           Psychologist psychologist = await _context.Psychologist.FindAsync(id);
            Client client = await _context.Client.FindAsync(idC);
            ViewBag.Psychologist_id = id;
            ViewBag.ClientID = idC;
            return View("Choose");
            // return View(psychologist);
        }


       
        [HttpPost]
        public async Task<ActionResult> ConfirmAsync(int id, int idC , DateTime Date_Session, Format Format_Session)
        {
            GetSession session=new GetSession();
            session.Date_Session = Date_Session;
            session.Format_Session = Format_Session;
            Psychologist psychologist = await _context.Psychologist.FindAsync(id);
            Client client = await _context.Client.FindAsync(idC);
            // Methods methods = await _context.Methods.FindAsync(psychologist.Methods_obj);          
            session.Psychologist_obj = psychologist;
            session.Psychologist_obj.Methods_obj = psychologist.Methods_obj;
            session.Psychologist_obj.Specialization_obj = psychologist.Specialization_obj;
            session.Psychologist_objId = id;
            session.Client = client;
            session.ClientID = idC;
            session.Status_Session = Status.Wait;
            _context.Add(session);
            _context.SaveChanges();
            ViewBag.Date_Session = Date_Session.ToLongDateString() + " " + Date_Session.ToShortTimeString();
            ViewBag.Format = Format_Session;

            session = _context
                .GetSession
                .Include(s => s.Psychologist_obj)
                .ThenInclude(p => p.Specialization_obj)
                .Include(s => s.Psychologist_obj)
                .ThenInclude(p => p.Methods_obj)
                .First(s => s.Session_ID == session.Session_ID);
           // ViewData["Format_Session"] = new SelectList(_context., "Methods_ID", "Methods_Name", psychologist.Methods_objId);
            return View(session);          
        }

   
        public IActionResult Confirm()
        {
            ViewData["Methods_objId"] = new SelectList(_context.Methods, "Methods_ID", "Methods_Name");
            ViewData["Specialization_objId"] = new SelectList(_context.Specialization, "Special_ID", "Special_Name");
            return RedirectToAction("Index", "Home");
        }




        private bool PsychologistExists(int id)
        {
          return _context.Psychologist.Any(e => e.ID == id);
        }


       
    }
}
