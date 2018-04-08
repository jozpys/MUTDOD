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
        INTERFACE_DECLARATION,
        PARENT_CLASSES,
        ATTRIBUTE_DECLARATION,
        ALTER_CLASS,
        ALTER_INTERFACE,
        DROP_ATTRIBUTE,
        DROP_CLASS,
        DROP_INTERFACE,
        NEW_OBJECT,
        OBJECT_INITIALIZATION_ELEMENT,
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
