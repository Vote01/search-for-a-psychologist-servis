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
using servis.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using servis.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Authentication;

namespace servis.Controllers
{
    [Authorize(Roles = "admin, psych")]
    public class PsychologistsController : Controller
    {
        private readonly PsychologistDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        private readonly SignInManager<servisUser> _signInManager;
        private readonly UserManager<servisUser> _userManager;
        private readonly IUserStore<servisUser> _userStore;
        private readonly IUserEmailStore<servisUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
     

        public PsychologistsController(PsychologistDBContext context, 
            IWebHostEnvironment appEnvironment,
             UserManager<servisUser> userManager,
            IUserStore<servisUser> userStore,
            SignInManager<servisUser> signInManager,
            ILogger<RegisterModel> logger,
            // IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
        }
        
        public string ReturnUrl { get; set; }


        public IList<AuthenticationScheme> ExternalLogins { get; set; }

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
            if (!String.IsNullOrEmpty(psychologist.Photo))
            {
                byte[] photodata =
               System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + psychologist.Photo);
                ViewBag.Photodata = photodata;
            }
            else
            {
                ViewBag.Photodata = null;
            }

            ViewData["Methods_objId"] = new SelectList(_context.Methods, "Methods_ID", "Methods_Name", psychologist.Methods_objId);
            ViewData["Specialization_objId"] = new SelectList(_context.Specialization, "Special_ID", "Special_Name", psychologist.Specialization_objId);
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
        public async Task<IActionResult> Create([Bind("ID,Name,LastName,Year,Info,Price,Methods_objId,Specialization_objId, Email, Phone, Profile, Password")] Psychologist psychologist, IFormFile upload)
        {

          //  psychologist.Profile = false;
           // psychologist.Password = null;
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
                    psychologist.Photo = path;
                }
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

            if (!String.IsNullOrEmpty(psychologist.Photo))
            {
                byte[] photodata = System.IO.File.ReadAllBytes(_appEnvironment.WebRootPath + psychologist.Photo);

                ViewBag.Photodata = photodata;
            }
            else
            {
                ViewBag.Photodata = null;
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,LastName,Year,Info,Price,Methods_objId,Specialization_objId, Email, Phone, Profile, Password")] Psychologist psychologist, IFormFile? upload)
        {
            if (id != psychologist.ID)
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
                    if (!String.IsNullOrEmpty(psychologist.Photo))
                    {
                        System.IO.File.Delete(_appEnvironment.WebRootPath +psychologist.Photo);
                    }
                    psychologist.Photo = path;
                }

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


        public async Task<IActionResult> Profile(int? id)
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
            ViewBag.ID = psychologist.ID;

            return View(psychologist);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(int ID, [Bind("ID,Name,LastName,Year,Info,Price,Methods_objId,Specialization_objId, Email, Phone, Photo, Profile, Password")] Psychologist psychologist)
        {
            //var psychologist = await _context.Psychologist.FindAsync(ID);
            
            if (psychologist == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                    var user = CreateUser();
                    user.ModelID = psychologist.ID;
                    user.FirstName = psychologist.Name;
                    user.LastName = psychologist.LastName;
                    user.Year = psychologist.Year;
                    user.Phone = psychologist.Phone;

                    await _userStore.SetUserNameAsync(user, psychologist.Email, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, psychologist.Email, CancellationToken.None);
                    var result = await _userManager.CreateAsync(user,psychologist.Password);

                    //returnUrl ??= Url.Content("~/");
                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                    if (result.Succeeded)
                    {
                        if (!await _roleManager.RoleExistsAsync("psych"))
                           await _roleManager.CreateAsync(new IdentityRole("psych"));
                        
                        await _userManager.AddToRoleAsync(user, "psych");
                        _logger.LogInformation("User created a new account with password.");

                        var userId = await _userManager.GetUserIdAsync(user);


                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        psychologist.Profile = true;
                       _context.Update(psychologist);
                         await _context.SaveChangesAsync();
                    //  return LocalRedirect(returnUrl);
                }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                  
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
           // return View(psychologist);
        }
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }
        private servisUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<servisUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(servisUser)}'. " +
                    $"Ensure that '{nameof(servisUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<servisUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<servisUser>)_userStore;
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

        public FileResult GetReport()
        {
           
            string path = "/Reports/templates/report_template_psychologist.xlsx";
            string result = "/Reports/report_psychologist.xlsx";
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + path);
            FileInfo fr = new FileInfo(_appEnvironment.WebRootPath + result);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                excelPackage.Workbook.Properties.Author = "Симонова А.М.";
                excelPackage.Workbook.Properties.Title = "Список психологов";
                excelPackage.Workbook.Properties.Subject = "Психологи";
                excelPackage.Workbook.Properties.Created = DateTime.Now;
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Psychologists"];
                int startLine = 3;
                List<Psychologist> psychologists = _context.Psychologist.Include(p => p.Methods_obj)
                .Include(p => p.Specialization_obj).ToList();
                foreach (Psychologist ps in psychologists)
                {

                    worksheet.Cells[startLine, 1].Value = startLine - 2;
                    worksheet.Cells[startLine, 2].Value = ps.ID;
                    worksheet.Cells[startLine, 3].Value = ps.Name;
                    worksheet.Cells[startLine, 4].Value = ps.LastName;
                    worksheet.Cells[startLine, 5].Value = ps.Year; 
                    worksheet.Cells[startLine, 6].Value = ps.Info;
                    worksheet.Cells[startLine, 7].Value = ps.Price; 
                    worksheet.Cells[startLine, 8].Value = ps.Methods_obj.Methods_Name;
                    worksheet.Cells[startLine, 9].Value = ps.Specialization_obj.Special_Name;
                    worksheet.Cells[startLine, 10].Value = ps.Email; 
                    worksheet.Cells[startLine, 11].Value = ps.Phone;
                    startLine++;
                }
                excelPackage.SaveAs(fr);
            }
            string file_type = "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
            string file_name = "report_psychologist.xlsx";
            return File(result, file_type, file_name);
        }



    }




}
