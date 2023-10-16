using Nibelung.Api.Models.Dto.Comment;

namespace Nibelung.Api.Models.Dto.Pagination
{
    public class PaginationResult<T> where T : class
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int PagesCount { get; set; }
        public int TotalCountItems { get; set; }
        public List<T>? Data { get; set; }
    }
}
