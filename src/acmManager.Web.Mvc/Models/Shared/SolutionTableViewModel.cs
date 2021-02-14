namespace acmManager.Web.Models.Shared
{
    public class SolutionTableViewModel : Problem.IndexViewModel
    {
        /// <summary>
        /// 若 Search 为 false，则假定不会使用 IndexViewModel.Filter
        /// </summary>
        public bool Search { get; set; }
    }
}