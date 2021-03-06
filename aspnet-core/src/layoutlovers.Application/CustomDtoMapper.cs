using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityProperties;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using IdentityServer4.Extensions;
using layoutlovers.Amazon;
using layoutlovers.Auditing.Dto;
using layoutlovers.Authorization.Accounts.Dto;
using layoutlovers.Authorization.Delegation;
using layoutlovers.Authorization.Permissions.Dto;
using layoutlovers.Authorization.Roles;
using layoutlovers.Authorization.Roles.Dto;
using layoutlovers.Authorization.Users;
using layoutlovers.Authorization.Users.Delegation.Dto;
using layoutlovers.Authorization.Users.Dto;
using layoutlovers.Authorization.Users.Importing.Dto;
using layoutlovers.Authorization.Users.Profile.Dto;
using layoutlovers.Categories;
using layoutlovers.Categories.Dto;
using layoutlovers.Chat;
using layoutlovers.Chat.Dto;
using layoutlovers.DynamicEntityProperties.Dto;
using layoutlovers.Editions;
using layoutlovers.Editions.Dto;
using layoutlovers.Favorites;
using layoutlovers.Favorites.Dto;
using layoutlovers.Files.Dto;
using layoutlovers.FilterTags;
using layoutlovers.FilterTags.Dto;
using layoutlovers.Friendships;
using layoutlovers.Friendships.Cache;
using layoutlovers.Friendships.Dto;
using layoutlovers.LayoutProducts;
using layoutlovers.LayoutProducts.Dto;
using layoutlovers.Localization.Dto;
using layoutlovers.MultiTenancy;
using layoutlovers.MultiTenancy.Accounting.Dto;
using layoutlovers.MultiTenancy.Dto;
using layoutlovers.MultiTenancy.HostDashboard.Dto;
using layoutlovers.MultiTenancy.Payments;
using layoutlovers.MultiTenancy.Payments.Dto;
using layoutlovers.MultiTenancy.Payments.Stripe.Dto;
using layoutlovers.Notifications.Dto;
using layoutlovers.Organizations.Dto;
using layoutlovers.Sessions.Dto;
using layoutlovers.WebHooks.Dto;
using Stripe;
using System.Linq;

namespace layoutlovers
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            configuration.CreateMap<FilterTag, FilterTagDto>()
                .ForMember(dto => dto.Name, opt => opt.MapFrom(ent => ent.Name))
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id));

            configuration.CreateMap<FilterTagDto, FilterTag>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(ent => ent.Id))
                .ForMember(dto => dto.Name, opt => opt.MapFrom(ent => ent.Name));

            configuration.CreateMap<CreateFilterTagDto, FilterTag>()
                .ForMember(dto => dto.Name, opt => opt.MapFrom(ent => ent.Name));

            configuration.CreateMap<Category, CategoryDto>();
            configuration.CreateMap<BuyProductCard, PaymentCardDto>();
            configuration.CreateMap<PaymentCardDto, TokenCardOptions>();
            configuration.CreateMap<PaymentCardDto, BuyProduct>();
            configuration.CreateMap<BuyProduct, PaymentCardDto>();

            configuration.CreateMap<CreateLayoutProductDto, LayoutProduct>();
            configuration.CreateMap<LayoutProduct, CreateLayoutProductDto>();
            configuration.CreateMap<AmazonS3File, S3FileDtoBase>();

            configuration.CreateMap<AmazonS3File, S3ImageDto>();
            configuration.CreateMap<AmazonS3File, S3FileDto>();
            
            configuration.CreateMap<FavoriteDto, Favorite>();
            configuration.CreateMap<Favorite, FavoriteDto>()
                .ForMember(dto => dto.ProductName, opt => opt.MapFrom(ent => ent.LayoutProduct.Name))
                .ForMember(dto => dto.Thumbnail, opt => opt.MapFrom(ent => ent.LayoutProduct.AmazonS3Files.FirstOrDefault(f => f.IsImage)))
                .ForMember(dto => dto.FileExtensions, opt => opt.MapFrom(ent => ent.LayoutProduct.AmazonS3Files.Where(f => !f.IsImage).Select(f => f.FileExtension)));
            configuration.CreateMap<CreateFavoriteDto, Favorite>();
            configuration.CreateMap<UpdateFavoriteDto, Favorite>();
            

            configuration.CreateMap<LayoutProduct, LayoutProductDto>()
                .ForMember(dto => dto.Category, opt => opt.MapFrom(ent => ent.Category))
                .ForMember(dto => dto.FilterTagDtos, opt => opt.MapFrom(ent => ent.ProductFilterTags.Select(f => f.FilterTag)))
                .ForMember(dto => dto.AmazonS3Files, opt => opt.MapFrom(ent => ent.AmazonS3Files));
            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();
            
            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();


            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicProperty, DynamicPropertyDto>().ReverseMap();
            configuration.CreateMap<DynamicPropertyValue, DynamicPropertyValueDto>().ReverseMap();
            configuration.CreateMap<DynamicEntityProperty, DynamicEntityPropertyDto>()
                .ForMember(dto => dto.DynamicPropertyName,
                    options => options.MapFrom(entity => entity.DynamicProperty.DisplayName.IsNullOrEmpty() ? entity.DynamicProperty.PropertyName : entity.DynamicProperty.DisplayName));
            configuration.CreateMap<DynamicEntityPropertyDto, DynamicEntityProperty>();

            configuration.CreateMap<DynamicEntityPropertyValue, DynamicEntityPropertyValueDto>().ReverseMap();
            
            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}
