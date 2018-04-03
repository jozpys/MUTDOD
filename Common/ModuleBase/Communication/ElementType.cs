using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTDOD.Common.ModuleBase.Communication
{
    public enum ElementType
    {
        SYSTEM_OPERATION,
        SYSTEM_INFO,
        CREATE_DATABASE,
        RENAME_DATABASE,
        DROP_DATABASE,
        CLASS_DECLARATION,
        ATTRIBUTE_DECLARATION,
        SELECT,
        CLASS_NAME,
        WHERE,
        IS_NULL,
        IS_NOT_NULL,
        COMPERISION,
        LEFT_OPERAND,
        RIGHT_OPERAND,
        OPERATOR,
        CLASS_PROPERTY,
        LITERAL,
        DATA_TYPE
    }
}
