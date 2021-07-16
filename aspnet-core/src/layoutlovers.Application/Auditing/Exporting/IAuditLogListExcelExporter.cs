using System.Collections.Generic;
using layoutlovers.Auditing.Dto;
using layoutlovers.Dto;

namespace layoutlovers.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
