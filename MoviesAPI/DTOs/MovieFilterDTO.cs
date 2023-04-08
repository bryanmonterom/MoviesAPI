namespace MoviesAPI.DTOs
{
    public class MovieFilterDTO
    {
        public int Page { get; set; } = 1;
        public int RecordsQuantityPerPage { get; set; } = 10;

        public PaginationDTO Pagination
        {
            get
            {
                return new PaginationDTO() { Page = Page, RecordsQuantityPerPage = RecordsQuantityPerPage };

            }
        }

        public string Title { get; set; }
        public int GenreId { get; set; }
        public bool InTheaters { get; set; }
        public bool NextLaunches { get; set; }
        public string SortField { get; set; }
        public bool SortOrder { get; set; }
    }
}
