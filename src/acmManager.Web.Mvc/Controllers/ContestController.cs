using System.Threading.Tasks;
using acmManager.Contest;
using acmManager.Controllers;
using acmManager.Web.Models.Contest;
using Microsoft.AspNetCore.Mvc;

namespace acmManager.Web.Controllers
{
    public class ContestController : acmManagerControllerBase
    {
        private readonly ContestAppService _contestAppService;

        public ContestController(ContestAppService contestAppService)
        {
            _contestAppService = contestAppService;
        }

        public async Task<ActionResult> Index()
        {
            var contestList = await _contestAppService.GetContestListAsync();
            return View("Index", new ContestListViewModel(contestList));
        }

        [HttpGet, Route("/Contest/{contestId}")]
        public async Task<ActionResult> GetContest(long contestId)
        {
            var contest = await _contestAppService.GetContestAsync(contestId);
            return View("Contest", contest);
        }

        [HttpGet, Route("/Contest/Create")]
        public ActionResult CreateContestView()
        {
            return View("CreateContest");
        }
        
    }
}