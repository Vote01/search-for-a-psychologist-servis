using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace servis.Models
{
    [ActivatorUtilitiesConstructor]
    public class GetSession
    {
        [Key]
        public int Session_ID { get; set; }
        
        [Display(Name = "Время сессии")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime Date_Session { get; set; }
        [Display(Name = "Формат")]

        //public bool Format_Session { get; set; }
        public Format Format_Session { get; set; }
        [Display(Name = "Психолог")]
        public int Psychologist_objId { get; set; }
        [Display(Name = "Психолог")]
        public Psychologist? Psychologist_obj { get; set; }

        [Display(Name = "Клиент")]
        public int ClientID { get; set; }
        [Display(Name = "Клиент")]
        public Client? Client { get; set; }

        [Display(Name = "Статус")]
        public Status Status_Session { get; set; }

    }


   public enum Format
    {
        [Display(Name = "Онлайн")]
        Online,
        [Display(Name = "Лично")]
        Offline
    }
    public enum Status
    {
        [Display(Name = "Ожидается")]
        Wait,
        [Display(Name = "Завершена")]
        [Description("Завершена")]
        Completed,
        //[Description("Отменена")]
        [Display(Name = "Отменена")]
        Cancelled
    }

}
