using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using acmManager.Authorization;
using acmManager.Controllers;
using acmManager.Problem;
using acmManager.Problem.Dto;
using acmManager.Web.Models.Problem;
using acmManager.Web.Models.Shared;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;

namespace acmManager.Web.Controllers
{
    public class ProblemController : acmManagerControllerBase
    {
        private readonly ProblemAppService _problemAppService;
        private readonly ArticleController _articleController;

        public ProblemController(ProblemAppService problemAppService, ArticleController articleController)
        {
            _problemAppService = problemAppService;
            _articleController = articleController;
        }

        public const int PageSize = 30;

        #region Pages

        [UnitOfWork]
        [AbpMvcAuthorize]
        [HttpGet, Route("/Problem/Solution/CommentTo/{solutionId}")]
        public async Task<ActionResult> CommentTo(long solutionId, long replyToCommentId)
        {
            var solution = await _problemAppService.GetSolution(solutionId);

            var reply = await _articleController.ReplyToComment(replyToCommentId);

            return View("Template/Comment/CommentTo", new CommentToViewModel
            {
                ArticleId = solution.SolutionId,
                CommentTitle = solution.SolutionTitle,
                CommentToLink = Url.Action("GetSolution", new {solutionId}),
                ReplyToCommentId = reply.Second,
                ReturnUrl = Url.Action("GetSolution", new {solutionId}),
                ContentInit =reply.First 
            });
        }

        [HttpGet, Route("/Problem/Solution")]
        public async Task<ActionResult> Index(int page, int user, string keyword)
        {
            var filter = new GetAllSolutionFilter
            {
                UserId = user,
                Keyword = keyword ?? "",
                TypeIds = keyword.IsNullOrEmpty()
                    ? null
                    : (await _problemAppService.GetAllProblemTypes(keyword)).Select(t => t.Id),
                MaxResultCount = PageSize,
                SkipCount = (page <= 1 ? 0 : page - 1) * PageSize 
            };
            var res = await _problemAppService.GetAllSolutionWithFilter(filter);
            
            return View("Index", new IndexViewModel
            {
                Solutions = res,
                Filter = filter
            });
        }

        [HttpGet, Route("/Problem/Solution/Edit/{solutionId}")]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Problem)]
        public async Task<ActionResult> Edit(long solutionId)
        {
            var solution = await _problemAppService.GetSolution(solutionId);

            if (solution.CreatorId != AbpSession.GetUserId())
            {
                throw new AbpAuthorizationException("Permission Denied");
            }

            return View(solution);
        }

        [HttpGet, Route("/Problem/Solution/Edit")]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Problem)]
        public ViewResult Edit()
        {
            return View(null);
        }

        [HttpGet, Route("/Problem/Solution/{solutionId}")]
        public async Task<ViewResult> GetSolution(long solutionId)
        {
            var solution = await _problemAppService.GetSolution(solutionId);

            return View(solution);
        }

        #endregion


        #region Apis

        [HttpPost, Route("/Problem/Solution/Create")]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Problem)]
        public async Task<JsonResult> Create(string name, string url, string description, string title, string  content, List<long> typeIds)
        {
            var id = await _problemAppService.CreateProblemSolution(new CreateSolutionInput
            {
                Name = name,
                Url = url,
                Description = description,
                TypeIds = typeIds,
                Title = title,
                Content = content
            });

            return Json(new {RedirectUrl = Url.Action("GetSolution", new {solutionId = id})});
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
            
            return Json(new {RedirectUrl = Url.Action("GetSolution", new {solutionId = id})});
        }

        [HttpGet, Route("/Problem/Types/GetAll")]
        public async Task<JsonResult> GetAllProblemTypes()
        {
            var res = await _problemAppService.GetAllProblemTypes("");
            return new JsonResult(res);
        }

        [HttpPost, Route("/Problem/Solution/Delete")]
        [AbpMvcAuthorize(PermissionNames.PagesUsers_Problem)]
        public async Task<RedirectToActionResult> DeleteSolution(long id)
        {
            await _problemAppService.DeleteSolution(id);
            return RedirectToAction("Index");
        }
        
        #endregion
    }
}