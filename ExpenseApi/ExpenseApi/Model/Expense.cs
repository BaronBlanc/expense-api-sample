using System.Text.Json.Serialization;

namespace ExpenseApi.Model
{
    public class Expense : BaseEntity
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public ExpenseType Type { get; set; }
        public double Amount { get; set; }
        public Guid CurrencyId { get; set; }
        public string Comment { get; set; }

        // Navigation Properties
        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public Currency? Currency { get; set; }
    }
}
