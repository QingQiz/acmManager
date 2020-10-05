using acmManager.File;
using AutoMapper;

namespace acmManager.Certificate.Dto
{
    public class CertificateMapProfile : Profile
    {
        public CertificateMapProfile()
        {
            CreateMap<UploadCertificateInput, Certificate>()
                .ForMember(cer => cer.File,
                    opt => opt.MapFrom(inp => FileAppService.SaveFormFileAsync(inp.File).Result));
        }
    }
}