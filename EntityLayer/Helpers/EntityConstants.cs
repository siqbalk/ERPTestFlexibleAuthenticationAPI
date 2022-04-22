using System;
using System.Collections.Generic;
using System.Text;

namespace EntityLayer.Helpers
{
    public class EntityConstants
    {
        public static class AuditType
        {
            public static readonly string Create = "Create";
            public static readonly string Update = "Update";
            public static readonly string Delete = "Delete";
        }

        public static class BaseEntityColumns
        {
            public static readonly string ModifiedBy = "ModifiedBy";
            public static readonly string ModifiedDate = "ModifiedDate";
            public static readonly string CreatedDate = "CreatedDate";
            public static readonly string CreatedBy = "CreatedBy";
            public static readonly string IsDeleted = "IsDeleted";
        }
    }
}
