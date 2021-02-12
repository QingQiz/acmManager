﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Abp.Runtime.Session;
using acmManager.Article;
using acmManager.Article.Dto;
using acmManager.Authorization;
using acmManager.Controllers;
using acmManager.Problem;
using acmManager.Problem.Dto;
using acmManager.Web.Models.Problem;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;

namespace acmManager.Web.Controllers
{
    public class ProblemController : acmManagerControllerBase
    {
        private readonly ProblemAppService _problemAppService;
        private readonly ArticleAppService _articleAppService;

        public ProblemController(ProblemAppService problemAppService, ArticleAppService articleAppService)
        {
            _problemAppService = problemAppService;
            _articleAppService = articleAppService;
        }

        public const int PageSize = 30;

        #region Pages

        [AbpMvcAuthorize]
        [HttpGet, Route("/Problem/Solution/CommentTo/{solutionId}")]
        public async Task<ActionResult> CommentTo(long solutionId, long replyToCommentId)
        {
            var solution = await _problemAppService.GetSolution(solutionId);
            return View("CommentTo", new CommentToSolutionViewModel
            {
                Solution = solution,
                ReplyToCommentId = replyToCommentId
            });
        }

        [HttpGet, Route("/Problem/Solution")]
        public async Task<ActionResult> Index(string keyword)
        {
            return await Index(1, 0, keyword);
        }
        
        [HttpGet, Route("/Problem/Solution/P{page}")]
        public async Task<ActionResult> Index(int page, int user, string keyword)
        {
            var filter = new GetAllSolutionFilter
            {
                UserId = user,
                KeyWords = keyword ?? "",
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

        [HttpPost, Route("/Problem/Solution/Comment")]
        [AbpMvcAuthorize]
        public async Task<RedirectToActionResult> CommentToSolution(long solutionId, long replyToCommentId, string content)
        {
            var solution = await _problemAppService.GetSolution(solutionId);
            await _articleAppService.CommentToArticle(new CommentToArticleInput
            {
                ReplyToCommentId = replyToCommentId,
                ArticleId = solution.SolutionId,
                Content = content
            });

            return RedirectToAction("GetSolution", new {solutionId});
        }
        
        #endregion
    }
}