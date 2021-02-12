using acmManager.Problem.Dto;

namespace acmManager.Web.Models.Problem
{
    public class CommentToSolutionViewModel
    {
        public GetSolutionOutput Solution;
        public long ReplyToCommentId;
    }
}