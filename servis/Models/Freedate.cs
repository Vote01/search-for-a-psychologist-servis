using System.ComponentModel.DataAnnotations;

namespace servis.Models
{
    public class Freedate
    {
        [Key]
        public int Freedate_ID { get; set; }

        List<DateTime> freetime { get; set; } //свободное время
        List<Tuple<DateTime>> freeday { get; set; }//свободный день




    }
}
