using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace servis.Models
{
    public class Client
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Имя")]
        public string? Name { get; set; }
        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }
        [Display(Name = "Возраст")]
        public int Year { get; set; }
        [Display(Name = "Почта")]
        public string? Email { get; set; }
        [Display(Name = "Телефон")]
        public string? Phone { get; set; }

        public string? Photo { get; set; }
    }
}
