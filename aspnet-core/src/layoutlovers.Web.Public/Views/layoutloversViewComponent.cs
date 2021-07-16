using Abp.AspNetCore.Mvc.ViewComponents;

namespace layoutlovers.Web.Public.Views
{
    public abstract class layoutloversViewComponent : AbpViewComponent
    {
        protected layoutloversViewComponent()
        {
            LocalizationSourceName = layoutloversConsts.LocalizationSourceName;
        }
    }
}