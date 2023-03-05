using AutoMapper;
using FeedbackDService.Data.Context.Entities;
using FeedbackDService.Requests;

namespace FeedbackDService.Mapper;

/// <summary>
/// 
/// </summary>
public class MapperProfile
{
    /// <summary>
    /// 
    /// </summary>
    public class Authorization : Profile
    {
        /// <summary>
        /// Конструктор клааса. В нём объявляются все карты для маппинга
        /// </summary>
        public Authorization()
        {
            CreateMap<AuthenticationRequest, User>()
                .ForMember(member => member.PasswordHash,
                    configuration => configuration.MapFrom(src => src.Password));
            
            CreateMap<RegistrationWithRoleRequest, User>()
                .ForMember(member => member.PasswordHash, 
                    configuration => configuration.MapFrom(src => src.Password));
        }
    }
    
    /// <summary>
    /// Профиль для категорий команий
    /// </summary>
    public class CompanyCategoryProfile : Profile
    {
        /// <summary>
        /// Конструктор клааса. В нём объявляются все карты для маппинга
        /// </summary>
        public CompanyCategoryProfile()
        {
            CreateMap<CreateCompanyCategoryRequest, CompanyCategory>();
        }
    }
    
    /// <summary>
    /// Профиль для компаний
    /// </summary>
    public class CompanyProfile : Profile
    {
        /// <summary>
        /// Конструктор клааса. В нём объявляются все карты для маппинга
        /// </summary>
        public CompanyProfile()
        {
            CreateMap<AddCompanyRequest, Company>();
        }
    }
}