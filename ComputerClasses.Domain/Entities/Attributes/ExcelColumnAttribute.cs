using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Import_and_Export.Entities.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ExcelColumnAttribute : Attribute
    {
        public string ColumnName { get; }
        public int Order { get; }

        public ExcelColumnAttribute(string columnName, int order = 0)
        {
            ColumnName = columnName;
            Order = order;
        }
    }
}
