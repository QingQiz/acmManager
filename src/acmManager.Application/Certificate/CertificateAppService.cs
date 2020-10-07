using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using acmManager.Authorization;
using acmManager.Certificate.Dto;
using acmManager.File;

namespace acmManager.Certificate
{
    public class CertificateAppService : acmManagerAppServiceBase
    {
        private readonly CertificateManager _certificateManager;
        private readonly FileManager _fileManager;

        public CertificateAppService(CertificateManager certificateManager, FileManager fileManager)
        {
            _certificateManager = certificateManager;
            _fileManager = fileManager;
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Certificate_Upload)]
        public virtual async Task UploadCertificateAsync(UploadCertificateInput input)
        {
            if (input.File.Length > 50 * 1024 * 1024)
            {
                throw new UserFriendlyException("file length must less than 50 MB");
            }

            var certificate = ObjectMapper.Map<Certificate>(input);
            await _certificateManager.Create(certificate);
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Certificate)]
        public virtual async Task UpdateCertificateAsync(UpdateCertificateInput input)
        {
            var certificate = await _certificateManager.Get(input.CertificateId);
            ObjectMapper.Map(input, certificate);
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Certificate)]
        public virtual async Task DeleteCertificateAsync(long certificateId)
        {
            var cer = await _certificateManager.GetWithFile(certificateId);
            if (cer.CreatorUserId != AbpSession.GetUserId())
            {
                throw new UserFriendlyException("Permission Denied");
            }
            await _fileManager.Delete(cer.File.Id);
            await _certificateManager.Delete(certificateId);
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Certificate)]
        public virtual async Task<List<GetCertificateOutput>> GetAllCertificateAsync()
        {
            var currentUserId = AbpSession.GetUserId();
            var res = await _certificateManager.GetAllWithFile(c => c.CreatorUserId == currentUserId);
            return ObjectMapper.Map<List<GetCertificateOutput>>(res);
        }
    }
}