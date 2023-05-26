using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace servis.Models
{
    public class PsychologistFiltr
    {
        public IEnumerable<Psychologist> Psychologist { get; set; }
        public SelectList MethodsP { get; set; }
        public SelectList SpecializationP { get; set; }
        public string Name { get; set; }

    }
}
