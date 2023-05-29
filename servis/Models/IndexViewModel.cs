namespace servis.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Psychologist> Users { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public PsychologistFiltr FilterViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }


    }
}
