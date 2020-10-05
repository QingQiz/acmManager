using Abp.AutoMapper;

namespace acmManager.Certificate.Dto
{
    [AutoMapTo(typeof(Certificate))]
    public class UpdateCertificateInput
    {
        public long CertificateId { get; set; }
        public string Name { get; set; }
        public string AwardDate { get; set; }
        public CertificateLevel Level { get; set; }
    }
}