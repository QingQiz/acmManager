using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using acmManager.Article;
using acmManager.Article.Dto;
using acmManager.Authorization;
using acmManager.Problem.Dto;

namespace acmManager.Problem
{
    public class ProblemAppService : acmManagerAppServiceBase
    {
        #region init

        private readonly ProblemManager _problemManager;
        private readonly ProblemSolutionManager _problemSolutionManager;
        private readonly ProblemTypeManager _problemTypeManager;
        private readonly ProblemToTypeManager _problemToTypeManager;
        private readonly ArticleAppService _articleAppService;
        

        public ProblemAppService(ProblemManager problemManager, ProblemSolutionManager problemSolutionManager, ProblemTypeManager problemTypeManager, ArticleAppService articleAppService, ProblemToTypeManager problemToTypeManager)
        {
            _problemManager = problemManager;
            _problemSolutionManager = problemSolutionManager;
            _problemTypeManager = problemTypeManager;
            _articleAppService = articleAppService;
            _problemToTypeManager = problemToTypeManager;
        }

        #endregion

        #region not map to remote

        [RemoteService(false)]
        private GetAllProblemSolutionList SolutionToDto(ProblemSolution solution)
        {
            return new GetAllProblemSolutionList
            {
                Id = solution.Id,
                ProblemName = solution.Problem.Name,
                ProblemUrl = solution.Problem.Url,
                ArticleTitle = solution.Solution.Title,
                CreatorUserId = solution.CreatorUserId ?? 0,
                ProblemTypes = solution.Problem.Types.Select(t =>
                    ObjectMapper.Map<ProblemTypeDto>(
                        _problemTypeManager.Get(t.ProblemTypeId).Result))
            };
        }
        

        #endregion

        #region problem type

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Problem)]
        public virtual async Task<long> CreateProblemType(ProblemTypeDto input)
        {
            return await _problemTypeManager.Create(ObjectMapper.Map<ProblemType>(input));
        }
        
        [UnitOfWork]
        public virtual async Task<IEnumerable<ProblemTypeDto>> GetAllProblemTypes(string keyword)
        {
            keyword ??= "";
            keyword = keyword.ToLower();
            var res = await _problemTypeManager
                .GetAll(pt =>
                    pt.Name.ToLower().Contains(keyword) ||
                    pt.Description.ToLower().Contains(keyword));
            return res.Select(ObjectMapper.Map<ProblemTypeDto>);
        }

        #endregion

        #region CRUD for solution
        
        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Problem)]
        public virtual async Task<long> CreateProblemSolution(CreateSolutionInput input)
        {
            var problem = ObjectMapper.Map<Problem>(input);
            problem.Types = new List<ProblemToType>();
            var article = ObjectMapper.Map<Article.Article>(input);

            var solution = new ProblemSolution
            {
                Problem = problem,
                Solution = article,
            };

            var pId = await _problemSolutionManager.Create(solution);
            
            foreach (var tid in input.TypeIds)
            {
                problem.Types.Add(new ProblemToType
                {
                    ProblemId = pId,
                    ProblemTypeId = tid,
                });
            }

            return pId;
        }

        [UnitOfWork]
        public virtual async Task<GetAllSolutionOutput> GetAllSolutionWithFilter(GetAllSolutionFilter filter)
        {
            filter.KeyWords ??= "";

            var containsKw = new Func<string, bool>(a
                => a.Contains(filter.KeyWords));
            var query = _problemSolutionManager.MakeQuery().AsEnumerable()
                .WhereIf(filter.KeyWords != "", s =>
                    containsKw(s.Problem.Name) ||
                    containsKw(s.Problem.Url) ||
                    containsKw(s.Problem.Description) ||
                    containsKw(s.Solution.Title) ||
                    containsKw(s.Solution.Content))
                .WhereIf(filter.TypeIds != null, s =>
                    s.Problem.Types.Any(t => filter.TypeIds.Contains(t.ProblemTypeId)));
            
            return await Task.Run(() =>
            {
                var problemSolutions = query as ProblemSolution[] ?? query.ToArray();
                return new GetAllSolutionOutput
                {
                    Solutions = problemSolutions
                        .Skip(filter.SkipCount)
                        .Take(filter.MaxResultCount)
                        .OrderByDescending(s => s.CreationTime)
                        .Select(SolutionToDto),
                    AllResultCount = problemSolutions.Count(),
                };
            });
        }

        [UnitOfWork]
        public virtual async Task<IEnumerable<GetAllProblemSolutionList>> GetAllSolutionByUser(long userId)
        {
            return await Task.Run(
                () => _problemSolutionManager
                    .MakeQuery()
                    .Where(s => s.CreatorUserId == userId)
                    .OrderByDescending(s => s.CreationTime)
                    .Select(SolutionToDto));
        }

        [UnitOfWork]
        public virtual async Task<GetSolutionOutput> GetSolution(long id)
        {
            var res = await _problemSolutionManager.Get(id);
            return new GetSolutionOutput
            {
                Id = res.Id,
                
                ProblemId = res.Problem.Id,
                ProblemName = res.Problem.Name,
                ProblemUrl = res.Problem.Url,
                ProblemDescription = res.Problem.Description,
                ProblemTypes = res.Problem.Types.Select(a 
                    => ObjectMapper.Map<ProblemTypeDto>(
                        _problemTypeManager.Get(a.ProblemTypeId).Result)),
                
                SolutionId = res.Solution.Id,
                SolutionTitle = res.Solution.Title,
                SolutionContent = res.Solution.Content,
                
                CreatorId = res.CreatorUserId ?? 0,
                
                Comments = ObjectMapper.Map<IEnumerable<CommentDto>>(res.Solution.Comments)
            };
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Problem)]
        public virtual async Task UpdateSolution(UpdateSolutionInput input)
        {
            var res = await _problemSolutionManager.Get(input.Id);

            if (AbpSession.GetUserId() != res.CreatorUserId)
            {
                throw new UserFriendlyException("Permission Denied");
            }
            
            res.Problem.Name = input.Name;
            res.Problem.Url = input.Url;
            res.Problem.Description = input.Description;
            
            var types = new List<ProblemToType>(res.Problem.Types);
            foreach (var t in types)
            {
                await _problemToTypeManager.Delete(t.Id);
            }
            res.Problem.Types.Clear();
            
            foreach (var tid in input.TypeIds)
            {
                res.Problem.Types.Add(new ProblemToType
                {
                    ProblemId = res.Problem.Id,
                    ProblemTypeId = tid
                });
            }

            res.Solution.Content = input.Content;
            res.Solution.Title = input.Title;
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Problem)]
        public virtual async Task DeleteSolution(long id)
        {
            var res = await _problemSolutionManager.Get(id);

            if (AbpSession.GetUserId() != res.CreatorUserId ||
                !await IsGrantedAsync(PermissionNames.PagesUsers_Problem_Delete))
            {
                throw new UserFriendlyException("Permission Denied");
            }

            var types = new List<ProblemToType>(res.Problem.Types);
            foreach (var t in types)
            {
                await _problemToTypeManager.Delete(t.Id);
            }

            await _problemManager.Delete(res.Problem.Id);
            await _articleAppService.DeleteArticleAsync(res.Solution.Id);
            await _problemSolutionManager.Delete(res.Id);
        }
        
        #endregion
    }
}