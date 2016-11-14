namespace SeamsCore.Features.PageSettings
{
    using AutoMapper;
    using SeamsCore.Domain;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Page, Load.Result>();
        }
    }
}
