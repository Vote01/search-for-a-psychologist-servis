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


        //[HttpPost]
        //public IActionResult Choose(int id)
        //{
        //    ViewData["Psychologist_objId"] = new SelectList(_context.Psychologist, "ID", "Name");
        //    return View();
        //}


        [HttpPost]
        public async Task<IActionResult> Choose(int id, DateTime Date_Session, bool Format_Session)
        {
           Psychologist psychologist = await _context.Psychologist.FindAsync(id);

            List<GetSession> sessions = _context.GetSession.Include(t => t.Psychologist_obj).ToList();
            //GetSession sessions = _context.GetSession
            //                           .Include(t => t.Psychologist_obj)
            //                           .ToList();
            ViewBag.Psychologist_id = id;
            ViewBag.Date_Session = Date_Session.ToLongDateString() + " " + Date_Session.ToShortTimeString();
            ViewBag.Format = Format_Session;

          
           // return View("Confirm", sessions);
            return View("Confirm");
        }


        [HttpPost]
        public ActionResult Confirm(int id, int Session_ID)
        {
            GetSession session = _context.GetSession.Find(Session_ID);
            session.Psychologist_objId = id;
            //_context.Entry(ticket).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


      

        
    






















    //[HttpPost]
    //public ActionResult Choose(int id, DateTime Date_Session, bool Format_Session)
    //// public ActionResult Choose(GetSession session)
    //{
    //    //GetSession session = _context.GetSession.Find(Session_ID);
    //    //session.Psychologist_objId = id;
    //    //_context.SaveChanges();
    //    //_context.Add(session);
    //    //_context.SaveChanges();
    //    ViewBag.Psychologist_id = id;
    //    ViewBag.Date_Session = Date_Session.ToLongDateString();
    //    ViewBag.Format = Format_Session;
    //    return View();
    //}


    //var result = new GetSessionsController().Create();



    //public IActionResult Confirm()
    //{

    //    ViewData["Psychologist_objId"] = new SelectList(_context.Psychologist, "ID", "Name");
    //    return View();
    //}


    // [HttpPost]
    
       //public async Task<IActionResult> Confirm([Bind("ID,Name,LastName,Year,Info,Price,Methods_objId,Specialization_objId")] GetSession session)
    
        
        //public async Task<IActionResult> Confirm(DateTime Date_Session, bool Format_Session, int id)
    //    //  public async Task<IActionResult> Confirm(int Session_ID)
    //         // public async Task<IActionResult> Confirm(GetSession session)
    //    {

    //       // _context.Add(session);
    //        _context.SaveChanges();
    //        //await _context.SaveChangesAsync();
    //        //GetSession session = _context.GetSession.Find(Session_ID);
    //        //ViewBag.Psychologist_id = session.Psychologist_objId;
    //        //ViewBag.Date_Session = session.Date_Session.ToLongDateString();
    //        //ViewBag.Format = session.Format_Session;

    //        //ViewData["Psychologist_objId"] = new SelectList(_context.GetSession, "ID", "Name", session.Psychologist_objId);

    //        //session.Psychologist_objId = id;
    //        //_context.SaveChanges();
    //        //Psychologist psychologist = await _context.Psychologist.FindAsync(id);
    //        //GetSession session = new GetSession();
    //        //  GetSession session = await _context.GetSession
    //        //.Include(t => t.Psychologist_objId)
    //        //.FirstOrDefaultAsync(m => m.Session_ID == Session_ID);

    //       // return View("Confirm", session);
    //        return RedirectToAction("Index", "Home");
    //       // return View(session);
    //    }




        //_context.Add(session);
        //        _context.SaveChanges();
        //       await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));


        //public async Task<IActionResult> Confirm(DateTime Date_Session, bool Format_Session, int id)
        //{
        

        //    Psychologist psychologist = await _context.Psychologist.FindAsync(id);
           
        //    GetSession Session = await _context.GetSession
        //  .Include(t => t.Date_Session).Include(t => t.Format_Session).Include(t => t.Psychologist_objId)
        //  .FirstOrDefaultAsync(m => m.Session_ID==Session_ID);

        //    ViewBag.Psychologist_id = id;
        //    ViewBag.Date_Session = Date_Session.ToLongDateString();
        //    ViewBag.Format = Format_Session;

        //    return View("Confirm",Session);
        //}

   

        //[HttpPost]

        //public ActionResult Confirm(int id, int Session_ID)
        //{
        //    GetSession session = _context.GetSession.Find(Session_ID);
        //    session.Psychologist_objId = id;       
        //    _context.SaveChanges();
        //    return RedirectToAction("Index", "Home");
        //    //return View();
        //}


        private bool PsychologistExists(int id)
        {
          return _context.Psychologist.Any(e => e.ID == id);
        }
    }
}
