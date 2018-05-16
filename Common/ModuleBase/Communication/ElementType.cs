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
        UPDATE_OBJECT,
        OBJECT_UPDATE_ELEMENT,
        DELETE_OBJECT,
        SELECT,
        DEREF,
        CLASS_NAME,
        WHERE,
        WHERE_OPERATION,
        LEFT_OPERAND,
        RIGHT_OPERAND,
        OPERATOR,
        CLASS_PROPERTY,
        LITERAL,
        DATA_TYPE
    }
}
