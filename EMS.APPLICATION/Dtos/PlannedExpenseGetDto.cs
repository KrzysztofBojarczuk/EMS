namespace EMS.APPLICATION.Dtos
{
    public class PlannedExpenseGetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTimeOffset DueDate { get; set; }
    }
}