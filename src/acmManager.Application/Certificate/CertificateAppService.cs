using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.UI;
using acmManager.Authorization;
using acmManager.Certificate.Dto;

namespace acmManager.Certificate
{
    public class CertificateAppService : acmManagerAppServiceBase
    {
        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Certificate_Upload)]
        public virtual async Task UploadCertificate(UploadCertificateInput input)
        {
            if (input.File.Length > 50 * 1024 * 1024)
            {
                throw new UserFriendlyException("file length must less than 50 MB");
            }

            var certificate = ObjectMapper.Map<Certificate>(input);
            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }
}