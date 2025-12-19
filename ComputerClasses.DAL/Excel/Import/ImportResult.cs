using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerClasses.Domain.Import
{
    public sealed record ImportError(int RowNumber, string ColumnName, string Message);

    public sealed class ImportResult<T>
    {
        public List<T> Items { get; } = [];
        public List<ImportError> Errors { get; } = [];
        public bool HasErrors => Errors.Count > 0;
    }
}
