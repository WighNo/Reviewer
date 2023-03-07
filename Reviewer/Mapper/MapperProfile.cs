using AutoMapper;
using Reviewer.Data.Context.Entities;
using Reviewer.Data.Requests;

namespace Reviewer.Mapper;

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
    
    /// <summary>
    /// Профиль для товаров и услуг
    /// </summary>
    public class ProductProfile : Profile
    {
        /// <summary>
        /// Конструктор клааса. В нём объявляются все карты для маппинга
        /// </summary>
        public ProductProfile()
        {
            CreateMap<AddProductRequest, Product>();
        }
    }
    
    /// <summary>
    /// Профиль для категорий товаров и услуг
    /// </summary>
    public class ProductCategoryProfile : Profile
    {
        /// <summary>
        /// Конструктор клааса. В нём объявляются все карты для маппинга
        /// </summary>
        public ProductCategoryProfile()
        {
            CreateMap<AddProductCategoryRequest, ProductCategory>();
        }
    }
}