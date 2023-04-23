using System.ComponentModel.DataAnnotations;

namespace servis.Models
{
    [ActivatorUtilitiesConstructor]
    public class GetSession
    {
        [Key]
        public int Session_ID { get; set; }
        
        [Display(Name = "Время сессии")]
        public DateTime Date_Session { get; set; }
        [Display(Name = "Формат")]
        public bool Format_Session { get; set; }
        [Display(Name = "Психолог")]
        public int Psychologist_objId { get; set; }
        [Display(Name = "Психолог")]
        public Psychologist? Psychologist_obj { get; set; }
       

    }
}
