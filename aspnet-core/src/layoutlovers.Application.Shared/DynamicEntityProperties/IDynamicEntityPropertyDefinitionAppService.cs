using System.Collections.Generic;

namespace layoutlovers.DynamicEntityProperties
{
    public interface IDynamicEntityPropertyDefinitionAppService
    {
        List<string> GetAllAllowedInputTypeNames();

        List<string> GetAllEntities();
    }
}
