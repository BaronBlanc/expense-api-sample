using System.Text.Json.Serialization;

namespace ExpenseApi.Model
{
    public class Currency : BaseEntity
    {
        public string Code { get; set; } // e.g., USD, EUR
        public string Name { get; set; } // e.g., US Dollar, Euro
        public string Symbol { get; set; } // e.g., $, €

        // Navigation Property
        [JsonIgnore]
        public ICollection<User> Users { get; set; }
        [JsonIgnore]
        public ICollection<Expense> Expenses { get; set; }
    }
}
