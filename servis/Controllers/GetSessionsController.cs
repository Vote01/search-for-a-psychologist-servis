using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using servis.Models;
using OfficeOpenXml;


namespace servis.Controllers
{
    public class GetSessionsController : Controller
    {
        private readonly PsychologistDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public GetSessionsController(PsychologistDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        //public GetSessionsController()
        //{
        //}

        public async Task<IActionResult> Index()
        {
            var psychologist = _context.GetSession.Include(g => g.Psychologist_obj);
            var client = _context.GetSession.Include(g => g.Client);
            return View(await psychologist.ToListAsync());
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
        public IActionResult Cancel()
        {
            ViewData["Psychologist_objId"] = new SelectList(_context.Psychologist, "ID", "Name");
            ViewData["Client"] = new SelectList(_context.Psychologist, "ID", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel([Bind("Session_ID,Date_Session,Format_Session,Psychologist_objId,ClientID, Status_Session")] GetSession getSession)
        {
            if (ModelState.IsValid)
            {
                getSession.Status_Session = Status.Cancelled;
                _context.Add(getSession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }    
            return View(getSession);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete([Bind("Session_ID,Date_Session,Format_Session,Psychologist_objId,ClientID, Status_Session")] GetSession getSession)
        {
            if (ModelState.IsValid)
            {
                getSession.Status_Session = Status.Completed;
                _context.Add(getSession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(getSession);
        }





        public IActionResult Create()
        {
            ViewData["Psychologist_objId"] = new SelectList(_context.Psychologist, "ID", "Name");
            return View();
        }

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
            // Путь к файлу с шаблоном
            string path = "/Reports/report_template.xlsx";
            //Путь к файлу с результатом
            string result = "/Reports/report.xlsx";
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + path);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + result);
            //будем использовть библитотеку не для коммерческого использования
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                excelPackage.Workbook.Properties.Author = "Симонова А.М.";
                excelPackage.Workbook.Properties.Title = "Список сессий";
                excelPackage.Workbook.Properties.Subject = "Сессии";
                excelPackage.Workbook.Properties.Created = DateTime.Now;
                //плучаем лист по имени.
                ExcelWorksheet worksheet =excelPackage.Workbook.Worksheets["GetSession"];
                int startLine = 3;
                List<GetSession> session = _context.GetSession.ToList();
                foreach (GetSession ses in session)
                {
                  //  Psychologist psychologist = _context.Psychologist.FindAsync(ses.Psychologist_objId);

                    worksheet.Cells[startLine, 1].Value = startLine - 2;
                    worksheet.Cells[startLine, 2].Value = ses.Session_ID;
                    worksheet.Cells[startLine, 3].Value = ses.Date_Session;
                    worksheet.Cells[startLine, 4].Value = ses.Format_Session;
                    worksheet.Cells[startLine, 5].Value = ses.Psychologist_objId;
                    //worksheet.Cells[startLine, 5].Value = ses.Psychologist_obj.Name; //работает ли
                    //worksheet.Cells[startLine, 6].Value = ses.Psychologist_obj.LastName;
                    startLine++;
                }
                excelPackage.SaveAs(fr);
            }
            string file_type = "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
            string file_name = "report.xlsx";
            return File(result, file_type, file_name);
        }

    }
}
