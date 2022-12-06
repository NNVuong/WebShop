using System;
namespace SharedObjects.ViewModels
{
    public class PaginationViewModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public int? CatId { get; set; }
        public string sortPrice { get; set; }
    }
}
