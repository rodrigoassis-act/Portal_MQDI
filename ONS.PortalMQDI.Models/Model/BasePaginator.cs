namespace ONS.PortalMQDI.Models.Model
{
    public class BasePaginator
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Offset
        {
            get { return (PageNumber - 1) * PageSize; }
        }
    }
}
