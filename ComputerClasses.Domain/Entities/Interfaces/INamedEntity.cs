using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Import_and_Export.Entities.Interfaces
{
    public interface INamedEntity
    {
        int Id { get; set; }
        string Name { get; set; }
    }
    
    
}
