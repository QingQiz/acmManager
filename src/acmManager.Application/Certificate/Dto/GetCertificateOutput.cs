using System;
using acmManager.File.Dto;

namespace acmManager.Certificate.Dto
{
    public class GetCertificateOutput
    {
        public long Id { get; set; }
        // 证书名
        public string Name { get; set; }

        // 证书级别
        public CertificateLevel Level { get; set; }

        public GetFileOutput File { get; set; }

        // 获奖日期
        public DateTime AwardDate { get; set; }
    }
}