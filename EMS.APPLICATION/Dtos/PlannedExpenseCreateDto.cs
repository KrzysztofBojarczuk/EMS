namespace EMS.APPLICATION.Dtos
{
    public class PlannedExpenseCreateDto
    {
        public string Name { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTimeOffset DueDate { get; set; }
    }
}
