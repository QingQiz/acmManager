using System.Collections.Generic;
using acmManager.Certificate.Dto;

namespace acmManager.Web.Models.Admin
{
    public class GetAllCertificateViewModel
    {
        public IEnumerable<GetCertificateOutput> Certificates;
    }
}