using Abp.AutoMapper;

namespace acmManager.Certificate.Dto
{
    [AutoMapFrom(typeof(Certificate))]
    public class GetAllCertificateSummary
    {
        public string Name { get; set; }
        public CertificateLevel Level { get; set; }
    }
}