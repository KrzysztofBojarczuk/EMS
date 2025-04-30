using EMS.CORE.Enums;

namespace EMS.APPLICATION.Dtos
{
    public class TransactionCreateDto
    {
        public string Name { get; set; } = null!;
        public DateTimeOffset CreationDate { get; set; }
        public CategoryType Category { get; set; }
        public decimal Amount { get; set; }
    }
}
