using System.ComponentModel.DataAnnotations;

namespace servis.Models
{
    public class Methods
    {
        [Key]
        public int Methods_ID { get; set; }
        [Display(Name = "Метод")]
        public string? Methods_Name { get; set; }

    }
}
