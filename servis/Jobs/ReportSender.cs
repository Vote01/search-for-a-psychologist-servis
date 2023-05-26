using System.Net.Mail;
using System.Net;
using servis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using OfficeOpenXml;
using Quartz;

namespace servis.Jobs
{
    public class ReportSender: Quartz.IJob
    {
        string file_path_template;
        string file_path_report;
        private readonly PsychologistDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        public ReportSender(PsychologistDBContext context, IWebHostEnvironment
       appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }
        public void PrepareReport()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            using (ExcelPackage excelPackage = new ExcelPackage(file_path_template))
            {
                excelPackage.Workbook.Properties.Author = "Симонова А.М.";
                excelPackage.Workbook.Properties.Title = "Список сессий";
                excelPackage.Workbook.Properties.Subject = "Сессии";
                excelPackage.Workbook.Properties.Created = DateTime.Now;
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Sessions"];
                int startLine = 3;
                List<GetSession> session = _context.GetSession.Include(g => g.Psychologist_obj)
                .Include(g => g.Client).ToList();
                foreach (GetSession ses in session)
                {
                    worksheet.Cells[startLine, 1].Value = startLine - 2;
                    worksheet.Cells[startLine, 2].Value = ses.Session_ID;
                    worksheet.Cells[startLine, 3].Value = ses.Date_Session.ToString("dd.MM.yyyy");
                    worksheet.Cells[startLine, 4].Value = ses.Format_Session;
                    worksheet.Cells[startLine, 5].Value = ses.Psychologist_obj.Name;
                    worksheet.Cells[startLine, 6].Value = ses.Psychologist_obj.LastName;
                    worksheet.Cells[startLine, 7].Value = ses.Client.Name; 
                    worksheet.Cells[startLine, 8].Value = ses.Client.LastName;
                    startLine++;
                }
                //созраняем в новое место
                excelPackage.SaveAs(file_path_report);
            }
        }
        public async Task Execute(IJobExecutionContext context)
        {
            // Путь к файлу с шаблоном
            string path = @"/Reports/templates/report_template_session.xlsx";
            //Путь к файлу с результатом
            string result = @"/Reports/report_session.xlsx";
            file_path_template = _appEnvironment.WebRootPath + path;
            file_path_report = _appEnvironment.WebRootPath + result;
            try
            {
                if (File.Exists(file_path_report))
                    File.Delete(file_path_report);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            PrepareReport();
            MailAddress from = new MailAddress("simonova.zlo@yandex.ru", "Al");
            MailAddress to = new MailAddress("simonova.zlo@yandex.ru");
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Отчёт";
            m.Body = "<h2>Отчёт по проведенным сессиям</h2>";
            m.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
            smtp.Credentials = new NetworkCredential("simonova.zlo@yandex.ru",
            "skqkthqlovpoyrsf");
            smtp.EnableSsl = true;
            m.Attachments.Add(new Attachment(file_path_report));
            await smtp.SendMailAsync(m);
            m.Dispose();
        }
    }
}

