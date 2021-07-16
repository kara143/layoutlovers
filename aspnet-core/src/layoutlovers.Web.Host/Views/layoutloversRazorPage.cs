using Abp.AspNetCore.Mvc.Views;

namespace layoutlovers.Web.Views
{
    public abstract class layoutloversRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected layoutloversRazorPage()
        {
            LocalizationSourceName = layoutloversConsts.LocalizationSourceName;
        }
    }
}
