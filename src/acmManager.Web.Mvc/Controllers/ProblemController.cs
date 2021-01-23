using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Abp.Runtime.Session;
using Abp.Web.Models;
using acmManager.Authorization;
using acmManager.Controllers;
using acmManager.Problem;
using acmManager.Problem.Dto;
using acmManager.Web.Models.Problem;
using Microsoft.AspNetCore.Mvc;

namespace acmManager.Web.Controllers
{
    public class ProblemController : acmManagerControllerBase
    {
        private readonly ProblemAppService _problemAppService;

        public ProblemController(ProblemAppService problemAppService)
        {
            _problemAppService = problemAppService;
        }

        [HttpGet, Route("/Problem/Solution")]
        public async Task<ActionResult> Index()
        {
            var filter = new GetAllSolutionFilter
            {
                KeyWords = "",
                TypeIds = new List<long>(),
                MaxResultCount = 30,
                SkipCount = 0
            };
            var res = await _problemAppService.GetAllSolutionWithFilter(filter);
            
            return View("Index", new IndexViewModel
            {
                Solutions = res,
                Filter = filter
            });
        }

        [HttpGet, Route("/Problem/Solution/Edit")]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Problem)]
        public async Task<ActionResult> Edit(long solutionId)
        {
            if (solutionId == 0) return View(null);
            
            var solution = await _problemAppService.GetSolution(solutionId);
                
            if (solution.CreatorId != AbpSession.GetUserId())
            {
                throw new AbpAuthorizationException("Permission Denied");
            }

            return View(solution);

        }

        [HttpPost, Route("/Problem/Solution/Create")]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Problem)]
        public async Task<JsonResult> Create(string name, string url, string description, string title, string  content, List<long> typeIds)
        {
            await _problemAppService.CreateProblemSolution(new CreateSolutionInput
            {
                Name = name,
                Url = url,
                Description = description,
                TypeIds = typeIds,
                Title = title,
                Content = content
            });
            return Json(new AjaxResponse());
        }
        
        [HttpPost, Route("/Problem/Solution/Update")]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Problem)]
        public async Task<JsonResult> Update(long id, string name, string url, string description, string title, string  content, List<long> typeIds)
        {
            await _problemAppService.UpdateSolution(new UpdateSolutionInput
            {
                Id = id,
                Name = name,
                Url = url,
                Description = description,
                TypeIds = typeIds,
                Title = title,
                Content = content
            });
            return Json(new AjaxResponse());
        }

        [HttpGet, Route("/Problem/Types/GetAll")]
        public async Task<JsonResult> GetAllProblemTypes()
        {
            var res = await _problemAppService.GetAllProblemTypes("");
            return new JsonResult(res);
        }
    }
}