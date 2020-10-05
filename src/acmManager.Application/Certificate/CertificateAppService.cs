using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using acmManager.Authorization;
using acmManager.Certificate.Dto;

namespace acmManager.Certificate
{
    public class CertificateAppService : acmManagerAppServiceBase
    {
        private CertificateManager _certificateManager;

        public CertificateAppService(CertificateManager certificateManager)
        {
            _certificateManager = certificateManager;
        }

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

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Certificate)]
        public virtual async Task UpdateCertificate(UpdateCertificateInput input)
        {
            var certificate = await _certificateManager.Get(input.CertificateId);
            ObjectMapper.Map(input, certificate);
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Certificate)]
        public virtual async Task DeleteCertificate(long certificateId)
        {
            await _certificateManager.Delete(certificateId);
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Certificate)]
        public virtual async Task<List<GetCertificateOutput>> GetAllCertificate()
        {
            var currentUserId = AbpSession.GetUserId();
            var res = await _certificateManager.GetAll(c => c.CreatorUserId == currentUserId);
            return ObjectMapper.Map<List<GetCertificateOutput>>(res);
        }
    }
}