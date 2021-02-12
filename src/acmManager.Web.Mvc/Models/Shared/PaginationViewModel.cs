namespace acmManager.Web.Models.Shared
{
    public class PaginationViewModel
    {
        public int MaxResultCount { get; set; }
        public int SkipCount { get; set; }
        
        public long AllResultCount { get; set; }
    }
}