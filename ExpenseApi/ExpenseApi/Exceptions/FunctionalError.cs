namespace ExpenseApi.Exceptions
{
    public static class FunctionalError
    {
        public const string USER_NOT_FOUND = "User not found";
        public const string EXPENSE_DATE_CANNOT_BE_IN_FUTURE = "Expense date cannot be in the future";
        public const string EXPENSE_DATE_CANNOT_BE_TOO_OLD = "Expense date cannot be older than 3 months";
        public const string COMMENT_REQUIRED = "Comment is required";
        public const string CURRENCY_DOES_NOT_MATCH = "Input currency does not match user's currency";
        public const string EXPENSE_ALREADY_EXIST = "Expense already exists for this user, date, and amount";
        public const string CURRENCY_NOT_FOUND = "Currency not found";
        public const string CANNOT_INTERPRET_TYPE = "Expense type entry is invalid. This field can only contains values Restaurant, Hotel, or Misc ";
        public const string SORT_EXPRESSION_NOT_VALID = "Could not interpret sort expression : Value must be date, type or amount";
    }
}
