using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using acmManager.Article.Dto;
using acmManager.Authorization;

namespace acmManager.Article
{
    public class ArticleAppService : acmManagerAppServiceBase
    {
        private readonly ArticleManager _articleManager;
        private readonly CommentManager _commentManager;
        private readonly BlogManager _blogManager;

        public const int ListContentLength = 500;

        public ArticleAppService(ArticleManager articleManager, CommentManager commentManager, BlogManager blogManager)
        {
            _articleManager = articleManager;
            _commentManager = commentManager;
            _blogManager = blogManager;
        }

        #region noMapped

        [RemoteService(false)]
        public static CommentDto CommentToDto(Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                CreationTime = comment.CreationTime,
                CreatorUserId = comment.CreatorUserId ?? AppConsts.FallBackUserId,
                ReplyToCommentId = comment.ReplyToCommentId,
                Content = comment.Content
            };
        }

        #endregion

        // in this region, all ARTICLE is the alias of BLOG
        #region blog
        
        [UnitOfWork]
        public virtual async Task<GetArticleListOutput> GetArticleWithFilter(GetArticleListFilter filter)
        {
            var query = _blogManager.GetAll().AsEnumerable()
                .WhereIf(filter.UserId != 0, a => a.CreatorUserId == filter.UserId)
                .WhereIf(!filter.Keyword.IsNullOrWhiteSpace(), a =>
                    a.Article.Content.Contains(filter.Keyword) || a.Article.Title.ToLower().Contains(filter.Keyword.ToLower()))
                .ToList();

            return await Task.Run(() => new GetArticleListOutput
            {
                AllResultCount = query.Count,
                Articles = query
                    .Skip(filter.SkipCount)
                    .Take(filter.MaxResultCount)
                    .Select(a => new GetArticleListDto
                    {
                        Id = a.Id,
                        
                        Title = a.Article.Title,
                        Content = string.Join("", a.Article.Content.Take(ListContentLength)),
                        Image = new Regex(@"!\[.*?\]\((.*?)\)").Match(a.Article.Content).Groups[1].Value,
                        
                        CreationTime = a.CreationTime,
                        CreatorUserId = a.CreatorUserId ?? AppConsts.FallBackUserId
                    })
                    .OrderByDescending(a => a.CreationTime)
            });
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Article)]
        public virtual async Task<long> CreateArticleAsync(CreateArticleInput input)
        {
            var blog = new Blog
            {
                Article = ObjectMapper.Map<Article>(input)
            };

            return await _blogManager.Create(blog);
        }

        [UnitOfWork]
        public virtual async Task<GetArticleOutput> GetArticleAsync(long blogId)
        {
            var blog = await _blogManager.Get(blogId);

            return new GetArticleOutput
            {
                Id = blogId,
                ArticleId = blog.Article.Id,
                Comments = blog.Article.Comments.Select(CommentToDto),
                CreationTime = blog.CreationTime,
                CreatorUserId = blog.CreatorUserId ?? AppConsts.FallBackUserId,
                Title = blog.Article.Title,
                Content = blog.Article.Content
            };
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Article)]
        public virtual async Task UpdateArticleAsync(UpdateArticleInput input)
        {
            var blog = await _blogManager.Get(input.Id);
            
            if (blog.CreatorUserId != AbpSession.GetUserId())
            {
                throw new UserFriendlyException("Permission Denied");
            }

            blog.Article.Content = input.Content;
            blog.Article.Title = input.Title;
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Article)]
        public virtual async Task DeleteArticleAsync(long blogId)
        {
            var blog = await _blogManager.Get(blogId);
            var article = blog.Article;
            
            if (article.CreatorUserId != AbpSession.GetUserId())
            {
                throw new UserFriendlyException("Permission Denied");
            }

            await _blogManager.Delete(blogId);
            
            // delete all comment
            foreach (var comment in article.Comments)
            {
                await _commentManager.Delete(comment.Id);
            }
            await _articleManager.Delete(article.Id);
        }

        #endregion
        

        #region comment

        /// <summary>
        /// Comment to an article
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        [AbpAuthorize]
        public virtual async Task CommentToArticle(CommentToArticleInput input)
        {
            var article = await _articleManager.Get(input.ArticleId);
            // comment will be created automatically
            article.Comments.Add(new Comment
            {
                Content = input.Content,
                // its no need to check if the comment exists
                // wrong id won't be displayed
                ReplyToCommentId = input.ReplyToCommentId
            });
        }

        /// <summary>
        /// Delete a comment
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        [UnitOfWork]
        [AbpAuthorize]
        public virtual async Task DeleteComment(long commentId)
        {
            var comment = await _commentManager.Get(commentId);
            
            // TODO administrator can delete everything
            if (AbpSession.GetUserId() != comment.CreatorUserId)
            {
                throw new UserFriendlyException("Permission Denied");
            }

            // remove from database
            await _commentManager.Delete(comment.Id);
        }
        

        #endregion
    }
}