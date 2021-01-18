using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Uow;
using acmManager.Article;
using acmManager.Problem.Dto;

namespace acmManager.Problem
{
    // TODO permissions
    public class ProblemAppService : acmManagerAppServiceBase
    {
        private readonly ProblemManager _problemManager;
        private readonly ProblemSolutionManager _problemSolutionManager;
        private readonly ProblemTypeManager _problemTypeManager;
        private readonly RecommendVoteManager _recommendVoteManager;
        private readonly ArticleManager _articleManager;
        

        public ProblemAppService(ProblemManager problemManager, ProblemSolutionManager problemSolutionManager, ProblemTypeManager problemTypeManager, RecommendVoteManager recommendVoteManager, ArticleManager articleManager)
        {
            _problemManager = problemManager;
            _problemSolutionManager = problemSolutionManager;
            _problemTypeManager = problemTypeManager;
            _recommendVoteManager = recommendVoteManager;
            _articleManager = articleManager;
        }

        [RemoteService(false)]
        private GetAllProblemSolutionList SolutionToDto(ProblemSolution solution)
        {
            return new GetAllProblemSolutionList
            {
                Id = solution.Id,
                Name = solution.Problem.Name,
                Url = solution.Problem.Url,
                CreatorUserId = solution.CreatorUserId ?? 0,
                ProblemTypes = ObjectMapper.Map<List<ProblemTypeDto>>(solution.Problem.Types),
                GoodVoteCnt = solution.RecommendVotes.Count(v => v.Type == VoteType.Good)
            };
        }

        [UnitOfWork]
        public virtual async Task<long> CreateProblemType(ProblemTypeDto input)
        {
            return await _problemTypeManager.Create(ObjectMapper.Map<ProblemType>(input));
        }

        [UnitOfWork]
        public virtual async Task CreateProblemSolution(CreateSolutionInput input)
        {
            var problem = ObjectMapper.Map<Problem>(input);
            var article = ObjectMapper.Map<Article.Article>(input);

            foreach (var tid in input.TypeIds)
            {
                problem.Types.Add(await _problemTypeManager.Get(tid));
            }
            article.Title = problem.Name;
            
            var solution = new ProblemSolution
            {
                Problem = problem,
                Solution = article,
                RecommendVotes = new List<RecommendVote>()
            };

            await _problemSolutionManager.Create(solution);
        }

        [UnitOfWork]
        public virtual async Task<GetAllSolutionOutput> GetAllSolutionWithFilter(GetAllSolutionFilter filter)
        {
            filter.KeyWords ??= "";
            var query = _problemSolutionManager.MakeQuery().AsEnumerable()
                .WhereIf(filter.KeyWords != "", s => s.Problem.Name.Contains(filter.KeyWords)
                                                     || s.Problem.Url.Contains(filter.KeyWords)
                                                     || s.Solution.Title.Contains(filter.KeyWords)
                                                     || s.Solution.Content.Contains(filter.KeyWords))
                .WhereIf(filter.TimeAfter != null, s => s.CreationTime > filter.TimeAfter)
                .WhereIf(filter.TypeIds.Any(), s => s.Problem.Types.Any(t => filter.TypeIds.Contains(t.Id)));
            
            return await Task.Run(() =>
            {
                var problemSolutions = query as ProblemSolution[] ?? query.ToArray();
                return new GetAllSolutionOutput
                {
                    Solutions = problemSolutions
                        .Skip(filter.SkipCount)
                        .Take(filter.MaxResultCount)
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
                    .AsEnumerable()
                    .Select(SolutionToDto));
        }

        [UnitOfWork]
        public virtual async Task<GetSolutionOutput> GetSolution(long id)
        {
            var res = await _problemSolutionManager.Get(id);
            return new GetSolutionOutput
            {
                Name = res.Problem.Name,
                Url = res.Problem.Url,
                Content = res.Solution.Content,
                GoodVoteCnt = res.RecommendVotes.Count(v => v.Type == VoteType.Good),
                Types = res.Problem.Types.Select(ObjectMapper.Map<ProblemTypeDto>)
            };
        }

        [UnitOfWork]
        public virtual async Task UpdateSolution(UpdateSolutionInput input)
        {
            var res = await _problemSolutionManager.Get(input.Id);
            res.Problem.Name = input.Name;
            res.Problem.Url = input.Url;
            res.Problem.Types.Clear();
            foreach (var tid in input.TypeIds)
            {
                res.Problem.Types.Add(await _problemTypeManager.Get(tid));
            }

            res.Solution.Content = input.Content;
            res.Solution.Content = input.Name;
        }
        
        [UnitOfWork]
        public virtual async Task<IEnumerable<ProblemTypeDto>> GetAllProblemTypes(string keyword)
        {
            keyword ??= "";
            var res = await _problemTypeManager
                .GetAll(pt => pt.Name.Contains(keyword)
                              || pt.Description.Contains(keyword));
            return res.Select(ObjectMapper.Map<ProblemTypeDto>);
        }

        [UnitOfWork]
        public virtual async Task DeleteSolution(long id)
        {
            var res = await _problemSolutionManager.Get(id);

            await _problemManager.Delete(res.Problem.Id);
            await _articleManager.Delete(res.Solution.Id);
            await _problemSolutionManager.Delete(res.Id);
        }

        /*
        [UnitOfWork]
        TODO public virtual async Task VoteSolution(VoteType voteType)
        TODO public virtual async Task Comment(long SolutionId, Comment comment)
        TODO public virtual async Task DeleteComment(long CommentId)
        */
    }
}