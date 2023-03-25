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


    }

   

    

 

}
