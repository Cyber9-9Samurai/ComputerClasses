using ComputerClasses.Domain.Entities;
using ComputerClasses.Domain.Entities.Attributes;

namespace ComputerClasses.Domain
{
    public class Row
    {
        [ExcelColumn("Идентификатор", 0)]
        public int Id { get; set; }
        [ExcelColumn("Факультет", 1)]
        public Facultie Facultie { get; set; }
        [ExcelColumn("Кафедра/Институт", 2)]
        public DepartmentOrInstitute DepartmentOrInstitute { get; set; }
        [ExcelColumn("Корпус", 3)]
        public Frame Frame { get; set; }
        [ExcelColumn("Номер аудитории", 4)]
        public AudienceNumber AudienceNumber { get; set; }
        [ExcelColumn("Название аудитории", 5)]
        public AudienceName AudienceName { get; set; }
        [ExcelColumn("Ответственный", 6)]
        public ResponsiblePerson ResponsiblePerson { get; set; }
        [ExcelColumn("Инвентарный номер", 7)]
        public InventoryNumber InventoryNumber { get; set; }
        [ExcelColumn("ОС", 8)]
        public Entities.OperatingSystem OperatingSystem { get; set; }
        [ExcelColumn("Материнская плата", 9)]
        public Motherboard Motherboard { get; set; }
        [ExcelColumn("ЦП", 10)]
        public Cpu Cpu { get; set; }
        [ExcelColumn("Видеокарта", 11)]
        public VideoCard VideoCard { get; set; }
        [ExcelColumn("Диски", 12)]
        public Disks Disk { get; set; }
        [ExcelColumn("ОЗУ, ГБ", 13)]
        public Ram Ram { get; set; }
        [ExcelColumn("Тип ОЗУ", 14)]
        public RamType RamType { get; set; }
        [ExcelColumn("Список установленных приложений", 15)]
        public InstalledApplications ApplicationList { get; set; }
        [ExcelColumn("Статус", 16)]
        public Status Status { get; set; }
        [ExcelColumn("Дата последнего обслуживания", 17)]
        public LastServiceDate LastServiceDate { get; set; }

    }
}
