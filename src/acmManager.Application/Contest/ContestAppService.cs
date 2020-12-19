using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using acmManager.Article;
using acmManager.Article.Dto;
using acmManager.Authorization;
using acmManager.Authorization.Users;
using acmManager.Contest.Dto;
using Microsoft.EntityFrameworkCore;

namespace acmManager.Contest
{
    public class ContestAppService : acmManagerAppServiceBase
    {
        private readonly ContestManager _contestManager;
        private readonly ArticleManager _articleManager;
        private readonly ContestSignUpManager _contestSignUpManager;
        private readonly UserManager _userManager;

        public ContestAppService(ContestManager contestManager, ArticleManager articleManager, ContestSignUpManager contestSignUpManager, UserManager userManager)
        {
            _contestManager = contestManager;
            _articleManager = articleManager;
            _contestSignUpManager = contestSignUpManager;
            _userManager = userManager;
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Contest)]
        public virtual async Task CreateContestAsync(CreateContestInput input)
        {
            var description = new Article.Article
            {
                Title = input.DescriptionTitle,
                Content = input.DescriptionContent
            };

            var contest = new Contest
            {
                Name = input.Name,
                Description = description,
                SignUpStartTime = input.SignUpStartTime,
                SignUpEndTime = input.SignUpEndTime,
                Result = null
            };

            await _contestManager.Create(contest);
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Contest)]
        public virtual async Task UpdateContestAsync(UpdateContestInput input)
        {
            var contest = await _contestManager.Get(input.Id);
            contest.Name = input.Name;
            contest.SignUpEndTime = input.SignUpEndTime;
            contest.SignUpStartTime = input.SignUpStartTime;
            contest.Description.Title = input.DescriptionTitle;
            contest.Description.Content = input.DescriptionContent;
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Contest)]
        public virtual async Task DeleteContestAsync(long id)
        {
            var contest = await _contestManager.Get(id);
            await _articleManager.Delete(contest.Description.Id);
            if (contest.Result != null) await _articleManager.Delete(contest.Result.Id);
            await _contestSignUpManager.DeleteAll(s => s.Contest.Id == id);
            await _contestManager.Delete(contest.Id);
        }

        [UnitOfWork]
        public virtual async Task<GetContestOutput> GetContestAsync(long contestId)
        {
            var contest = await _contestManager.Get(contestId);
            var res = new GetContestOutput
            {
                Description = ObjectMapper.Map<GetArticleOutput>(contest.Description),
                Result = contest.Result == null ? null : ObjectMapper.Map<GetArticleOutput>(contest.Result),
                Name = contest.Name,
                SignUpStartTime = contest.SignUpStartTime,
                SignUpEndTime = contest.SignUpEndTime,
                Id = contest.Id,
                SignUpInfo = null
            };

            if (AbpSession.UserId == null)
            {
                return res;
            }

            var query = _contestSignUpManager.MakeQuery(AbpSession.GetUserId(), contestId);
            if (!await query.AnyAsync()) return res;
            
            var user = await _userManager.GetUserByIdAsync(AbpSession.GetUserId());
            var password = user.UserInfo.StudentNumber + user.UserInfo.Name + user.UserInfo.ClassId;
            
            res.SignUpInfo = new ContestSignUpInfo
            {
                Name = user.UserInfo.StudentNumber,
                Password = BitConverter
                    .ToString(SHA1
                        .Create()
                        .ComputeHash(Encoding.UTF8.GetBytes(password)))
                    .Replace("-", "")
                    .Substring(0, 8)
            };

            return res;
        }
        
        [UnitOfWork]
        public virtual async Task<List<GetContestListOutput>> GetContestListAsync()
        {
            var contests = (await _contestManager.GetAll())
                .OrderByDescending(c => c.SignUpEndTime)
                .Take(30);
            return ObjectMapper.Map<List<GetContestListOutput>>(contests);
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Contest)]
        public virtual async Task SetContestResultAsync(SetContestResultInput input)
        {
            var contest = await _contestManager.Get(input.Id);

            if (contest.Result != null)
            {
                throw new UserFriendlyException("Contest DOES have a result");
            }
            
            contest.Result = new Article.Article
            {
                Title = input.Title,
                Content = input.Content
            };
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Contest_SignUp)]
        public virtual async Task ContestSignUpAsync(long contestId)
        {
            var contest = await _contestManager.Get(contestId);

            if (contest.SignUpEndTime < DateTime.Now || contest.SignUpStartTime > DateTime.Now)
            {
                throw new UserFriendlyException("Can not sign up to this contest now");
            }

            if (await _contestSignUpManager.Check(AbpSession.GetUserId(), contestId))
            {
                return;
            }
            await _contestSignUpManager.Create(new ContestSignUp
            {
                Contest = contest
            });
        }
    }
}