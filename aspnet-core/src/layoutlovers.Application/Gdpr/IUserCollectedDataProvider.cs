using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using layoutlovers.Dto;

namespace layoutlovers.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
