using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ComputerClasses.Domain.Entities
{
    public class Row
    {
        public int Id { get; set; }
        public Facultie Facultie { get; set; }
        public DepartmentOrInstitute DepartmentOrInstitute { get; set; }
        public Frame Frame { get; set; }
        public AudienceNumber AudienceNumber { get; set; }
        public AudienceName AudienceName { get; set; }
        public ResponsiblePerson ResponsiblePerson { get; set; }
        public InventoryNumber InventoryNumber { get; set; }
        public OperatingSystem OperatingSystem { get; set; }
        public Motherboard Motherboard { get; set; }
        public Cpu Cpu { get; set; }
        public VideoCard VideoCard { get; set; }
        public Disks Disk { get; set; }
        public Ram Ram { get; set; }
        public RamType RamType { get; set; }
        public InstalledApplications ApplicationList { get; set; }
        public Status Status { get; set; }
        public LastServiceDate LastServiceDate { get; set; }

    }
}
