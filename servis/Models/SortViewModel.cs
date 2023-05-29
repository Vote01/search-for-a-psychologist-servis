namespace servis.Models
{
    public class SortViewModel
    {
        public SortState PriceSort { get; private set; }   // значение для сортировки по компании
        public SortState Current { get; private set; }     // текущее значение сортировки

        public SortViewModel(SortState sortOrder)
        {
           
            PriceSort = sortOrder == SortState.PriceAsc ? SortState.PriceDesc: SortState.PriceAsc;
            Current = sortOrder;
        }


    }
}
