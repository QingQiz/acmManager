using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace acmManager.Certificate.Dto
{
    public class UploadCertificateInput
    {
        // 证书名
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        // 证书级别
        [Required]
        public CertificateLevel Level { get; set; }

        [Required]
        public IFormFile File { get; set; }

        // 获奖日期
        [Required]
        public DateTime AwardDate { get; set; }
    }
}