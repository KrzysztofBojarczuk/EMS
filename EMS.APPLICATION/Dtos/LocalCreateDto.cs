namespace EMS.APPLICATION.Dtos
{
    public class LocalCreateDto
    {
        public Guid Id { get; set; } //unikalny identyfikator
        public string Description { get; set; } = null!;
        public int LocalNumber { get; set; }
        public double Surface { get; set; }
        public bool NeedsRepair { get; set; }
    }
}
