using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using servis.Models;
using OfficeOpenXml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using servis.Areas.Identity.Data;

namespace servis.Controllers
{
    [Authorize]
    public class GetSessionsController : Controller
    {
        private readonly PsychologistDBContext _context;
        private readonly UserManager<servisUser> _userManager;
        private readonly IWebHostEnvironment _appEnvironment;

        public GetSessionsController(PsychologistDBContext context, UserManager<servisUser> userManager, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
        }

        //public GetSessionsController()
        //{
        //}

        public async Task<IActionResult> Index()
        {
            var psychologist = _context.GetSession.Include(g => g.Psychologist_obj);
            var client = _context.GetSession.Include(g => g.Client);
          
            var user = await _userManager.GetUserAsync(User);
            if (User.IsInRole("guest"))
            {


                var getSession = _context.GetSession
              .Include(g => g.Psychologist_obj)
              .Include(g => g.Client)
              .Where(v => v.ClientID == user.ModelID);

               // var appDBContext = _context.GetSession.Include(v => v.Doctor).Include(v => v.Patient).
               // Where(v => v.PatientId == user.ModelId).
               // Where(v => v.DateVisiting >= dates[0] && v.DateVisiting <= dates[1]);
                    return View(await getSession.ToListAsync());

            }
            else if (User.IsInRole("psych"))
            {
                var getSession = _context.GetSession
            .Include(g => g.Psychologist_obj)
            .Include(g => g.Client)
            .Where(v => v.Psychologist_objId == user.ModelID);
                return View(await getSession.ToListAsync());
            }
            else
            {
                var getSession = _context.GetSession
                   .Include(g => g.Psychologist_obj)
                   .Include(g => g.Client);

                return View(await getSession.ToListAsync());
            }

            //  return View(await psychologist.ToListAsync());

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GetSession == null)
            {
                return NotFound();
            }

            var getSession = await _context.GetSession
                .Include(g => g.Psychologist_obj)
                .Include(g => g.Client)
                .FirstOrDefaultAsync(m => m.Session_ID == id);
            if (getSession == null)
            {
                return NotFound();
            }

            return View(getSession);
        }

        public async Task<IActionResult> EditStatus(int? id)
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
            return View(getSession);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStatus(int id, [Bind("Session_ID,Date_Session,Format_Session,Psychologist_objId,ClientID,Status_Session,Feedback")] GetSession getSession, int ClientID, int Psychologist_objId)
        {

            getSession.Psychologist_obj= await _context.Psychologist.FindAsync(Psychologist_objId);
            getSession.Client= await _context.Client.FindAsync(ClientID);
           // getSession.Feedback = " ";
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
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> StatusComp(int? id)
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
            return View(getSession);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StatusComp(int id, [Bind("Session_ID,Date_Session,Format_Session,Psychologist_objId,ClientID,Status_Session,Feedback")] GetSession getSession, int ClientID, int Psychologist_objId)
        {

            getSession.Psychologist_obj = await _context.Psychologist.FindAsync(Psychologist_objId);
            getSession.Client = await _context.Client.FindAsync(ClientID);
       
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
          
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Create()
        {
            ViewData["Psychologist_objId"] = new SelectList(_context.Psychologist, "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Session_ID,Date_Session,Format_Session,Status_Session,Psychologist_objId")] GetSession getSession)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Session_ID,Date_Session,Format_Session,Status_Session ,Psychologist_objId")] GetSession getSession)
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

          //  await _context.SaveChangesAsync();
            ///return RedirectToAction(nameof(Index)); 
            return View(getSession);
        }

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

        public FileResult GetReport()
        {
            string path = "/Reports/templates/report_template_session.xlsx";
            string result = "/Reports/report_session.xlsx";
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + path);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + result);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                excelPackage.Workbook.Properties.Author = "Симонова А.М.";
                excelPackage.Workbook.Properties.Title = "Список сессий";
                excelPackage.Workbook.Properties.Subject = "Сессии";
                excelPackage.Workbook.Properties.Created = DateTime.Now;
                ExcelWorksheet worksheet =excelPackage.Workbook.Worksheets["Sessions"];
                int startLine = 3;
                List<GetSession> session = _context.GetSession.Include(g => g.Psychologist_obj)
                .Include(g => g.Client).ToList();
                foreach (GetSession ses in session)
                {
                    worksheet.Cells[startLine, 1].Value = startLine - 2;
                    worksheet.Cells[startLine, 2].Value = ses.Session_ID;
                    worksheet.Cells[startLine, 3].Value = ses.Date_Session.ToString("dd.MM.yyyy");
                    worksheet.Cells[startLine, 4].Value = ses.Format_Session;
                    worksheet.Cells[startLine, 5].Value = ses.Psychologist_obj.Name; //работает ли
                    worksheet.Cells[startLine, 6].Value = ses.Psychologist_obj.LastName;
                    worksheet.Cells[startLine, 7].Value = ses.Client.Name; //работает ли
                    worksheet.Cells[startLine, 8].Value = ses.Client.LastName;
                    startLine++;
                }
                excelPackage.SaveAs(fr);
            }
            string file_type = "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
            string file_name = "report_session.xlsx";
            return File(result, file_type, file_name);
        }

    }
}
