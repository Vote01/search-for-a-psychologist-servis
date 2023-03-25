using System.ComponentModel.DataAnnotations;

namespace servis.Models
{
    public class Session
    {
        public int Psychologist_ID { get; set; }
        public Psychologist? psychologist { get; set; }

        public int Session_ID { get; set; }
        
        public GetSession? getSession { get; set; }
       

    }
}
