using System.Collections.Generic;

namespace acmManager.Certificate.Dto
{
    public class GetAllCertificateOutput
    {
        public IEnumerable<GetCertificateOutput> Certificates { get; set; }
        public long AllResultCount { get; set; }
    }
}