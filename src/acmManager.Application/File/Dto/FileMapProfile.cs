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
                        => opt.MapFrom(f => f.RealPath.Substring(f.RealPath.IndexOf("Upload", StringComparison.Ordinal)).Replace('\\', '/')));
        }
    }
}