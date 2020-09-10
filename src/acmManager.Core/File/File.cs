using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;

namespace acmManager.File
{
    public class File: FullAuditedEntity<long>
    {
        [Required]
        public string UploadName { get; set; }
        [Required]
        public string RealPath { get; set; }
        public string MimeType { get; set; }
    }
}