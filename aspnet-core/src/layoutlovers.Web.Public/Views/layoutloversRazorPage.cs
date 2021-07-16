using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace layoutlovers.Web.Public.Views
{
    public abstract class layoutloversRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected layoutloversRazorPage()
        {
            LocalizationSourceName = layoutloversConsts.LocalizationSourceName;
        }
    }
}
