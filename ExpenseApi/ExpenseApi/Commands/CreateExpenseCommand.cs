﻿using ExpenseApi.Model;
using MediatR;

namespace ExpenseApi.Commands
{
    public class CreateExpenseCommand : IRequest<CreateExpenseResult>
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string Comment { get; set; }
    }
}