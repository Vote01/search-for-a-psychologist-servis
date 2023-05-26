using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using servis.Models;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml;

namespace servis.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly PsychologistDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public ClientsController(PsychologistDBContext context, IWebHostEnvironment appEnvironment)
        {      
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
              return _context.Client != null ? 
                          View(await _context.Client.ToListAsync()) :
                          Problem("Entity set 'PsychologistDBContext.Client'  is null.");
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }
            if (!String.IsNullOrEmpty(client.Photo))
            {
                byte[] photodata =
               System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + client.Photo);
                ViewBag.Photodata = photodata;
            }
            else
            {
                ViewBag.Photodata = null;
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,LastName,Year,Email,Phone")] Client client, IFormFile upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    string path = "/Files/" + upload.FileName;
                    using (var fileStream = new
                   FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await upload.CopyToAsync(fileStream);
                    }
                    client.Photo = path;
                }
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            if (!String.IsNullOrEmpty(client.Photo))
            {
                byte[] photodata = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + client.Photo);

                ViewBag.Photodata = photodata;
            }
            else
            {
                ViewBag.Photodata = null;
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,LastName,Year,Email,Phone")] Client client, IFormFile? upload)
        {
            if (id != client.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    string path = "/Files/" + upload.FileName;
                    using (var fileStream = new
                   FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await upload.CopyToAsync(fileStream);
                    }
                    if (!String.IsNullOrEmpty(client.Photo))
                    {
                        System.IO.File.Delete(_appEnvironment.WebRootPath + client.Photo);
                    }
                    client.Photo = path;
                }
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.ID))
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
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Client == null)
            {
                return Problem("Entity set 'PsychologistDBContext.Client'  is null.");
            }
            var client = await _context.Client.FindAsync(id);
            if (client != null)
            {
                _context.Client.Remove(client);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
          return (_context.Client?.Any(e => e.ID == id)).GetValueOrDefault();
        }

        public FileResult GetReport()
        {
            string path = "/Reports/templates/report_template_client.xlsx";
             string result = "/Reports/report_client.xlsx";
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + path);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + result);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                excelPackage.Workbook.Properties.Author = "Симонова А.М.";
                excelPackage.Workbook.Properties.Title = "Список клиентов";
                excelPackage.Workbook.Properties.Subject = "Клиенты";
                excelPackage.Workbook.Properties.Created = DateTime.Now;
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Clients"];
                int startLine = 3;
                List<Client> clients = _context.Client.ToList();
           
                foreach (Client cl in clients)
                {

                
                    worksheet.Cells[startLine, 1].Value = startLine - 2;
                    worksheet.Cells[startLine, 2].Value = cl.ID;
                    worksheet.Cells[startLine, 3].Value = cl.Name;
                    worksheet.Cells[startLine, 4].Value = cl.LastName;
                    worksheet.Cells[startLine, 5].Value = cl.Year; 
                    worksheet.Cells[startLine, 6].Value = cl.Email;
                    worksheet.Cells[startLine, 7].Value = cl.Phone; 
                    startLine++;
                }
                excelPackage.SaveAs(fr);
            }
            string file_type = "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
            string file_name = "report_client.xlsx";
            return File(result, file_type, file_name);
        }


    }
}
