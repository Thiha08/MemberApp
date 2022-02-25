namespace MemberApp.Data.Infrastructure.Core.Result
{
    public class PagedInfo
    {
        public PagedInfo(int pageNumber, int pageSize, int totalPages, int totalRecords)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
            TotalRecords = totalRecords;
        }

        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalRecords { get; private set; }

        public PagedInfo SetPageNumber(int pageNumber)
        {
            PageNumber = pageNumber;

            return this;
        }

        public PagedInfo SetPageSize(int pageSize)
        {
            PageSize = pageSize;

            return this;
        }

        public PagedInfo SetTotalPages(int totalPages)
        {
            TotalPages = totalPages;

            return this;
        }

        public PagedInfo SetTotalRecords(int totalRecords)
        {
            TotalRecords = totalRecords;

            return this;
        }
    }
}
