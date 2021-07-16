using System.Collections.Generic;
using layoutlovers.Authorization.Users.Importing.Dto;
using layoutlovers.Dto;

namespace layoutlovers.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
