using ComputerClasses.Domain.Entities.Interfaces;

namespace ComputerClasses.Domain.Entities
{
    public class LastServiceDate : INamedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}