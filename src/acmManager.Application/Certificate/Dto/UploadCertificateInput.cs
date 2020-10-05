using System;
using Microsoft.AspNetCore.Http;

namespace acmManager.Certificate.Dto
{
    public class UploadCertificateInput
    {
        // 证书名
        public string Name { get; set; }

        // 证书级别
        public CertificateLevel Level { get; set; }

        public IFormFile File { get; set; }

        // 获奖日期
        public DateTime AwardDate { get; set; }
    }
}