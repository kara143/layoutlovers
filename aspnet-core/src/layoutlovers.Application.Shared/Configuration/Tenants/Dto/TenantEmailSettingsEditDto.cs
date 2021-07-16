using Abp.Auditing;
using layoutlovers.Configuration.Dto;

namespace layoutlovers.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}