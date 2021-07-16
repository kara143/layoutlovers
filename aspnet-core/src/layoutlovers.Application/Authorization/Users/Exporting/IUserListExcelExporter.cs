using System.Collections.Generic;
using layoutlovers.Authorization.Users.Dto;
using layoutlovers.Dto;

namespace layoutlovers.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}