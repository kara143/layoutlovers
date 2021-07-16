using System.Collections.Generic;
using layoutlovers.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace layoutlovers.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
