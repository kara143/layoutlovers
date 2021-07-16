using System.Threading.Tasks;
using Abp.Dependency;

namespace layoutlovers.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}