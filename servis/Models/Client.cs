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
        [Required(ErrorMessage = "Введите электронную почту")]
        [Display(Name = "Почта")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Формат почты неправильный")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Введите номер телефона")]
        [Display(Name = "Телефон")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{1})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Формат номера неправильный")]
         //[RegularExpression(@"^\+?(\d[\d -. ]+)?(\([\d -. ]+\))?[\d-. ]+\d$", ErrorMessage = "Формат номера неправильный")]


        public string? Phone { get; set; }

        public string? Photo { get; set; }
    }
}
