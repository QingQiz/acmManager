﻿using System;
using System.IO;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.UI;
using acmManager.Authorization.Users;
using acmManager.File.Dto;
using Microsoft.AspNetCore.Http;

namespace acmManager.File
{
    public class FileAppService: acmManagerAppServiceBase
    {
        /// <summary>
        /// 用户上传图像
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        [AbpAuthorize]
        public async Task UploadUserPhotoAsync(IFormFile photo)
        {
            if (!photo.ContentType.Contains("image"))
            {
                throw new UserFriendlyException("Wrong file type: " + photo.ContentType);
            }

            if (photo.Length > 50 * 1024 * 1024) // 50 MB
            {
                throw new UserFriendlyException("file length must less than 50 MB");
            }
            
            var user = await GetCurrentUserAsync();
            user.UserInfo ??= new UserInfo();
            
            // YY/mm
            var subPath = Path.Combine(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString("D2"));
            // currentPath/App_Data/Uploads/YY/mm
            var fpRoot = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "App_Data/Uploads"), subPath);
            if (!Directory.Exists(fpRoot)) Directory.CreateDirectory(fpRoot);
            // currentPath/App_Data/Uploads/YY/mm/RandomFileName.Ext
            var realName = Guid.NewGuid() + Path.GetExtension(photo.FileName)?.ToLower();
            var realPath = Path.Combine(fpRoot, realName);

            await using (var stream = new FileStream(realPath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            user.UserInfo.Photo = new File()
            {
                UploadName = photo.FileName,
                RealPath = realPath,
                MimeType = photo.ContentType
            };
        }

        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="GetFileOutput"/>头像的URL应该map到静态资源的地址</returns>
        [AbpAuthorize]
        public async Task<GetFileOutput> GetUserPhotoAsync(long userId)
        {
            var user = await UserManager.GetUserByIdWithPhotoAsync(userId);

            return ObjectMapper.Map<GetFileOutput>(user.UserInfo.Photo);
        }
    }
}