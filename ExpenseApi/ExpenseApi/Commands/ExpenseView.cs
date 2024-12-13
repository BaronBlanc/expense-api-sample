namespace ExpenseApi.Commands
{
    public class ExpenseView
    {
        public Guid Id { get; set; }
        public string User { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string Comment { get; set; }
    }
}
