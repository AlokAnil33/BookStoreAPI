namespace BookStore.API.DataModels
{
    public class PaginationModel
    {
        private int pageNo = 1;

        public int PageNo
        {
            get { return Math.Max(1, pageNo); }
            set { pageNo = value; }
        }

        private int pageSize = 10;

        public int PageSize
        {
            get { return Math.Max(1, Math.Min(50, pageSize)); }
            set { pageSize = value; }

        }
        
    }
}
