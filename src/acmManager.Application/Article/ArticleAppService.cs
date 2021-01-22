﻿using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Uow;
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

        public ArticleAppService(ArticleManager articleManager, CommentManager commentManager)
        {
            _articleManager = articleManager;
            _commentManager = commentManager;
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Article)]
        public virtual async Task CreateArticleAsync(CreateArticleInput input)
        {
            await _articleManager.Create(ObjectMapper.Map<Article>(input));
        }

        [UnitOfWork]
        public virtual async Task<GetArticleOutput> GetArticleAsync(long articleId)
        {
            var article = await _articleManager.Get(articleId);
            article.ViewTimes += 1;
            return ObjectMapper.Map<GetArticleOutput>(article);
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Article)]
        public virtual async Task UpdateArticleAsync(UpdateArticleInput input)
        {
            var article = await _articleManager.Get(input.Id);
            if (article.CreatorUserId != AbpSession.GetUserId())
            {
                throw new UserFriendlyException("Permission Denied");
            }

            ObjectMapper.Map(input, article);
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Article)]
        public virtual async Task DeleteArticleAsync(long articleId)
        {
            var article = await _articleManager.Get(articleId);
            if (article.CreatorUserId != AbpSession.GetUserId())
            {
                throw new UserFriendlyException("Permission Denied");
            }

            // delete all comment
            foreach (var comment in article.Comments)
            {
                await _commentManager.Delete(comment.Id);
            }

            await _articleManager.Delete(articleId);
        }

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
        public virtual async Task DeleteComment(DeleteCommentInput input)
        {
            var comment = await _commentManager.Get(input.CommentId);
            var article = await _articleManager.Get(input.ArticleId);
            
            // TODO administrator can delete everything
            if (AbpSession.GetUserId() != comment.CreatorUserId)
            {
                throw new UserFriendlyException("Permission Denied");
            }

            // remove from article
            if (!article.Comments.Remove(comment))
            {
                throw new UserFriendlyException("No such comment");
            }
            
            // remove from database
            await _commentManager.Delete(comment.Id);
        }
        

        #endregion
    }
}