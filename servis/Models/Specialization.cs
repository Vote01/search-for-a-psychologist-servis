using System.ComponentModel.DataAnnotations;

namespace servis.Models
{
    public class Specialization
    {
        [Key]
        public int Special_ID { get; set; }
        [Display(Name = "Специализация")]
        public string? Special_Name { get; set; }
    }
}
