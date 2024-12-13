# Backend developer API

This is a .NET Web API application with a REST API to:
- Create expenses
- List expenses

## Specifications
### Resources
#### Expenses
An expense is characterized by:
- A user (person who made the purchase)
- A date
- A type (possible values: Restaurant, Hotel, and Misc)
- An amount and a currency
- A comment

#### Users
A user is characterized by:
- A last name
- A first name
- A currency in which the user makes their expenses

### Main features
#### Creating an expense

This REST API should allow:
 - Creating an expense considering the validation rules.

Expense validation rules:
- An expense cannot have a date in the future,
- An expense cannot be dated more than 3 months ago,
- The comment is mandatory,
- A user cannot declare the same expense twice (same date and amount),
- The currency of the expense must match the user’s currency.

#### Listing expenses
This REST API allows:
- Listing the expenses for a given user,
- Fetch all the existing users
- Sorting expenses by amount or date,
- Displaying all the properties of the expense; the user of the expense should appear in the format `{FirstName} {LastName}` (e.g., "Anthony Stark").

### Additional information
- Authentication management is not implemented.

### Storage
Data is persisted in an SQLite. Were are using an ORM to interact with the database. 

The users' table is initialized with the following users:

- Stark Anthony (with the currency being the U.S. dollar),
- Romanova Natasha (with the currency being the Russian ruble).

### I chose different design pattern :

 - Specification pattern : to handle specific user criteria in requests
 - Repository pattern with unit of work to wrap entity framework calls (except in the user Controller)
 - Command pattern
