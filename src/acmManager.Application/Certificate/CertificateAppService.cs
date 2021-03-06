﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using acmManager.Authorization;
using acmManager.Certificate.Dto;
using acmManager.File;
using Microsoft.EntityFrameworkCore;

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

        #region NotMapToRemote

        [RemoteService(false)]
        public GetAllCertificateOutput MakePage(IEnumerable<Certificate> query, int skip, int take)
        {
            var q = query.ToList();
            return new GetAllCertificateOutput
            {
                Certificates = q.Skip(skip).Take(take).Select(c => ObjectMapper.Map<GetCertificateOutput>(c)),
                AllResultCount = q.Count
            };
        }

        #endregion

        #region NormalApis

        /// <summary>
        /// 获取所有证书的简略信息，不需要验证
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<IEnumerable<GetAllCertificateSummary>> GetAllCertificateSummary(long userId=0)
        {
            return (await _certificateManager.GetAll())
                .WhereIf(userId != 0, c => c.CreatorUserId == userId)
                .Select(ObjectMapper.Map<GetAllCertificateSummary>);
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

        #endregion

        #region PrivilegeApis

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Certificate_DeleteAll)]
        public virtual async Task DeleteAsync(long id)
        {
            var cer = await _certificateManager.GetWithFile(id);
            await _fileManager.Delete(cer.File.Id);
            await _certificateManager.Delete(id);
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Certificate_GetAll)]
        public virtual async Task<IEnumerable<GetCertificateOutput>> GetByUserAsync(long userId)
        {
            var res = await _certificateManager.GetAllWithFile(c => c.CreatorUserId == userId);
            return ObjectMapper.Map<List<GetCertificateOutput>>(res);
        }

        [UnitOfWork]
        [AbpAuthorize(PermissionNames.PagesUsers_Certificate_GetAll)]
        public virtual async Task<GetAllCertificateOutput> GetWithFilter(GetAllCertificateWithFilterInput filterInput)
        {
            var emptyStr = new Func<string, bool>(string.IsNullOrEmpty);

            var query = _certificateManager.Certificates.Include(c => c.File)
                .WhereIf(!emptyStr(filterInput.Name), c => c.Name.Contains(filterInput.Name))
                .WhereIf(filterInput.Levels != null && filterInput.Levels.Any(),
                    c => filterInput.Levels.Contains(c.Level))
                .Where(c
                    => c.AwardDate >= (filterInput.TimeStart ?? DateTime.MinValue)
                       && c.AwardDate <= (filterInput.TimeEnd ?? DateTime.MaxValue));

            return await Task.Run(() => MakePage(query, filterInput.SkipCount, filterInput.MaxResultCount));
        }

        #endregion
    }
}