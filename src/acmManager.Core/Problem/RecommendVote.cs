using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using acmManager.Authorization.Users;

namespace acmManager.Problem
{
    [Table("acmMgr.RecommendVote")]
    public class RecommendVote: Entity<long>, ICreationAudited<User>
    {
        public VoteType Type { get; set; }
        
        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public User CreatorUser { get; set; }
    }
}