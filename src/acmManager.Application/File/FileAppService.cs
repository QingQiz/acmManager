using System;
using System.IO;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.UI;
using acmManager.Authorization.Users;
using acmManager.File.Dto;
using Microsoft.AspNetCore.Http;

namespace acmManager.File
{
    public class FileAppService : acmManagerAppServiceBase
    {
        [RemoteService(false)]
        public static async Task<File> SaveFormFileAsync(IFormFile input)
        {
            // YY/mm
            var subPath = Path.Combine(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString("D2"));
            // currentPath/App_Data/Uploads/YY/mm
            var fpRoot = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "App_Data/Uploads"), subPath);
            if (!Directory.Exists(fpRoot)) Directory.CreateDirectory(fpRoot);
            // currentPath/App_Data/Uploads/YY/mm/RandomFileName.Ext
            var realName = Guid.NewGuid() + Path.GetExtension(input.FileName)?.ToLower();
            var realPath = Path.Combine(fpRoot, realName);

            await using var stream = new FileStream(realPath, FileMode.Create);
            await input.CopyToAsync(stream);

            return new File
            {
                UploadName = input.Name,
                RealPath = realPath,
                MimeType = input.ContentType
            };
        }

        /// <summary>
        ///     用户上传图像
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        [AbpAuthorize]
        [UnitOfWork]
        public virtual async Task UploadUserPhotoAsync(IFormFile photo)
        {
            if (!photo.ContentType.Contains("image"))
                throw new UserFriendlyException("Wrong file type: " + photo.ContentType);

            if (photo.Length > 50 * 1024 * 1024) // 50 MB
                throw new UserFriendlyException("file length must less than 50 MB");

            var user = await GetCurrentUserAsync();
            user.UserInfo ??= new UserInfo();

            user.UserInfo.Photo = await SaveFormFileAsync(photo);
        }

        /// <summary>
        ///     获取用户头像
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="GetFileOutput" />头像的URL应该map到静态资源的地址</returns>
        [AbpAuthorize]
        public async Task<GetFileOutput> GetUserPhotoAsync(long userId)
        {
            var user = await UserManager.GetUserByIdWithPhotoAsync(userId);

            return ObjectMapper.Map<GetFileOutput>(user.UserInfo.Photo);
        }
    }
}