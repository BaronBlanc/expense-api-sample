using System.Text.Json.Serialization;

namespace ExpenseApi.Model
{
    public class User : BaseEntity
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public Guid CurrencyId { get; set; }

        // Navigation Property
        [JsonIgnore]
        public Currency? Currency { get; set; }

        // Navigation Property
        [JsonIgnore]
        public ICollection<Expense> Expenses { get; set; }
    }
}
