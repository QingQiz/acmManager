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

        [HttpGet, Route("/Contest/EditResult/{contestId}")]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Contest)]
        public async Task<ActionResult> EditContestResult(long contestId)
        {
            var contest = await _contestAppService.GetContestAsync(contestId);
            return View("EditContestResult", contest);
        }

        [HttpPost, Route("/Contest/EditResult/Post")]
        public async Task<ActionResult> EditContestResultPost(SetContestResultInput input)
        {
            if (input.Content == null)
            {
                await _contestAppService.RemoveContestResultAsync(input.Id);
            }
            else
            {
                await _contestAppService.SetContestResultAsync(input);
            }
           
            return RedirectToAction("GetContest",new { contestId = input.Id });
        }

        [HttpGet, Route("/Contest/{contestId}/SignUp")]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Contest_SignUp)]
        public async Task<ActionResult> ContestSignUp(long contestId)
        {
            await _contestAppService.ContestSignUpAsync(contestId);

            return RedirectToAction("GetContest", new {contestId});
        }

        [HttpGet, Route("/Contest/{contestId}/SignUp/Cancel")]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Contest_SignUp)]
        public async Task<ActionResult> ContestSignUpCancel(long contestId)
        {
            await _contestAppService.ContestSignUpCancelAsync(contestId);
            return RedirectToAction("GetContest", new {contestId});
        }

        [HttpGet, Route("/Contest/{contestId}/SignUp/List")]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Contest)]
        public async Task<ActionResult> ContestSignUpList(long contestId)
        {
            var suList = await _contestAppService.GetContestSignUpList(contestId);
            return View(new ContestSignUpListViewModel {SignUps = suList});
        }
    }
}