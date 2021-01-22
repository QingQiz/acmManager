using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Uow;
using acmManager.Article;
using acmManager.Article.Dto;
using acmManager.Problem.Dto;

namespace acmManager.Problem
{
    // TODO permissions
    public class ProblemAppService : acmManagerAppServiceBase
    {
        #region init

        private readonly ProblemManager _problemManager;
        private readonly ProblemSolutionManager _problemSolutionManager;
        private readonly ProblemTypeManager _problemTypeManager;
        private readonly ArticleAppService _articleAppService;
        

        public ProblemAppService(ProblemManager problemManager, ProblemSolutionManager problemSolutionManager, ProblemTypeManager problemTypeManager, ArticleAppService articleAppService)
        {
            _problemManager = problemManager;
            _problemSolutionManager = problemSolutionManager;
            _problemTypeManager = problemTypeManager;
            _articleAppService = articleAppService;
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
                ProblemTypes = ObjectMapper.Map<List<ProblemTypeDto>>(solution.Problem.Types),
            };
        }
        

        #endregion

        #region problem type

        [UnitOfWork]
        public virtual async Task<long> CreateProblemType(ProblemTypeDto input)
        {
            return await _problemTypeManager.Create(ObjectMapper.Map<ProblemType>(input));
        }
        
        [UnitOfWork]
        public virtual async Task<IEnumerable<ProblemTypeDto>> GetAllProblemTypes(string keyword)
        {
            keyword ??= "";
            var res = await _problemTypeManager
                .GetAll(pt =>
                    pt.Name.Contains(keyword) ||
                    pt.Description.Contains(keyword));
            return res.Select(ObjectMapper.Map<ProblemTypeDto>);
        }

        #endregion

        #region CRUD for solution
        
        [UnitOfWork]
        public virtual async Task CreateProblemSolution(CreateSolutionInput input)
        {
            var problem = ObjectMapper.Map<Problem>(input);
            var article = ObjectMapper.Map<Article.Article>(input);

            foreach (var tid in input.TypeIds)
            {
                problem.Types.Add(await _problemTypeManager.Get(tid));
            }
            
            var solution = new ProblemSolution
            {
                Problem = problem,
                Solution = article,
            };

            await _problemSolutionManager.Create(solution);
        }

        [UnitOfWork]
        public virtual async Task<GetAllSolutionOutput> GetAllSolutionWithFilter(GetAllSolutionFilter filter)
        {
            filter.KeyWords ??= "";
            var query = _problemSolutionManager.MakeQuery().AsEnumerable()
                .WhereIf(filter.KeyWords != "", s =>
                    s.Problem.Name.Contains(filter.KeyWords) ||
                    s.Problem.Url.Contains(filter.KeyWords) ||
                    s.Problem.Description.Contains(filter.KeyWords) ||
                    s.Solution.Title.Contains(filter.KeyWords) ||
                    s.Solution.Content.Contains(filter.KeyWords))
                .WhereIf(filter.TypeIds.Any(), s => s.Problem.Types.Any(t => filter.TypeIds.Contains(t.Id)));
            
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
                ProblemTypes = res.Problem.Types.Select(ObjectMapper.Map<ProblemTypeDto>),
                
                SolutionId = res.Solution.Id,
                SolutionTitle = res.Solution.Title,
                SolutionContent = res.Solution.Content,
                
                Comments = ObjectMapper.Map<IEnumerable<CommentDto>>(res.Solution.Comments)
            };
        }

        [UnitOfWork]
        public virtual async Task UpdateSolution(UpdateSolutionInput input)
        {
            var res = await _problemSolutionManager.Get(input.Id);
            res.Problem.Name = input.Name;
            res.Problem.Url = input.Url;
            res.Problem.Description = input.Description;
            res.Problem.Types.Clear();
            foreach (var tid in input.TypeIds)
            {
                res.Problem.Types.Add(await _problemTypeManager.Get(tid));
            }

            res.Solution.Content = input.Content;
            res.Solution.Content = input.Name;
        }

        [UnitOfWork]
        public virtual async Task DeleteSolution(long id)
        {
            var res = await _problemSolutionManager.Get(id);

            await _problemManager.Delete(res.Problem.Id);
            await _articleAppService.DeleteArticleAsync(res.Solution.Id);
            await _problemSolutionManager.Delete(res.Id);
        }
        
        #endregion
        
        //TODO Permission Checker
    }
}