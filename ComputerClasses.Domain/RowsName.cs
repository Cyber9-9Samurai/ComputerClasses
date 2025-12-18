using ComputerClasses.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Test_Import_and_Export.Entities.Attributes;

namespace ComputerClasses.Domain
{
    public static class RowsName
    {
        public static readonly List<string> Names = GenerateColumnNames();
        private static List<string> GenerateColumnNames()
        {
            var properties = typeof(Row)
                .GetProperties()
                .Where(p => p.GetCustomAttribute<ExcelColumnAttribute>() != null)
                .ToArray();

            var sorted = properties
                .Select(p => new
                {
                    Property = p,
                    Attribute = p.GetCustomAttribute<ExcelColumnAttribute>()!
                })
                .OrderBy(x => x.Attribute.Order)
                .ToArray();

            return [.. sorted.Select(x => x.Attribute.ColumnName)];
        }
    }
}
