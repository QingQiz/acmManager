using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using acmManager.Authorization;
using acmManager.Contest;
using acmManager.Contest.Dto;
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

        [HttpGet, Route("/Contest/Edit")]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Contest)]
        public async Task<ActionResult> EditContest(long contestId)
        {
            if (contestId != 0)
            {
                var contest = await _contestAppService.GetContestAsync(contestId);
                return View("EditContest", contest);
            }
            else
            {
                return View("EditContest", null);
            }
        }

        [HttpPost, Route("/Contest/Create")]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Contest)]
        public async Task<JsonResult> CreateContest(CreateContestInput input)
        {
            await _contestAppService.CreateContestAsync(input);
            return Json(new AjaxResponse());
        }

        [HttpPost, Route("/Contest/Update")]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Contest)]
        public async Task<JsonResult> UpdateContest(UpdateContestInput input)
        {
            await _contestAppService.UpdateContestAsync(input);
            return Json(new AjaxResponse());
        }

        [HttpPost]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Contest)]
        public async Task<ActionResult> DeleteContest(long contestId)
        {
            await _contestAppService.DeleteContestAsync(contestId);
            return RedirectToAction("Index");
        }
    }
}