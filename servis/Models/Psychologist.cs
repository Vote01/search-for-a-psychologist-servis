using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace servis.Models
{
    public class Psychologist
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Имя")]
        public string? Name { get; set; }
        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }
        [Display(Name = "Возраст")]
        public int Year { get; set; }
        [Display(Name = "Дополнительная информация")]
        public string? Info { get; set; }
        [Display(Name = "Цена сеанса")]
        public int Price { get; set; }
        [Display(Name = "Метод")]
        public int Methods_objId { get; set; }
        [Display(Name = "Метод")]
        public Methods? Methods_obj { get; set; }
        [Display(Name = "Специализация")]
        public int Specialization_objId { get; set; }
        [Display(Name = "Специализация")]
        public Specialization? Specialization_obj { get; set; }

        [Required(ErrorMessage = "Введите электронную почту")]
        [Display(Name = "Почта")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Формат почты неправильный")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Введите номер телефона")]
        [Display(Name = "Телефон")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{1})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Формат номера неправильный")]
        public string? Phone { get; set; }
        public string? Photo { get; set; }

        public bool? Profile { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }

    }

   

    

 

}
