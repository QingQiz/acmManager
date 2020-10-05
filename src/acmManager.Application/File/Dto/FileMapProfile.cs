using System;
using AutoMapper;

namespace acmManager.File.Dto
{
    public class FileMapProfile: Profile
    {
        public FileMapProfile()
        {
            CreateMap<File, GetFileOutput>()
                .ForMember(o => o.FileName
                    , opt => opt.MapFrom(f => f.UploadName))
                .ForMember(o => o.FilePath
                    , opt
                        => opt.MapFrom(f => RealPathToVirtualPath(f.RealPath)));
        }

        public static string RealPathToVirtualPath(string realPath)
        {
            return "/" + realPath.Substring(realPath.IndexOf("Upload", StringComparison.Ordinal)).Replace('\\', '/');
        }
    }
}