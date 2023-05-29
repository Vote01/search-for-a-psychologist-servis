using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace servis.Models
{
    public class PsychologistFiltr
    {

        public PsychologistFiltr(List<Methods> method, int? psych, string name)
        {
           // List<Specialization> specializations,
            method.Insert(0, new Methods { Methods_Name = "Все", Methods_ID = 0 });
            MethodsP = new SelectList(method, "Methods_ID", "Methods_Name", psych);
          //  specializations.Insert(0, new Specialization { Special_Name = "Все", Special_ID = 0 });
          //  MethodsP = new SelectList(method, "ID", "Name", psych);
            SelectedPs = psych;
            Name = name;
        }

       // public IEnumerable<Psychologist> Psychologist { get; set; }
        public SelectList MethodsP { get; set; }
        public SelectList SpecializationP { get; set; }
        public int? SelectedPs { get; private set; }   
        public string Name { get; set; }

    }
}
