using System.Collections.Generic;
using Abp;
using layoutlovers.Chat.Dto;
using layoutlovers.Dto;

namespace layoutlovers.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
