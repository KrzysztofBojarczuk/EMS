﻿using EMS.APPLICATION.Dtos;
using EMS.CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CORE.Entities
{
    public class TransactionEntity
    {
        public Guid Id { get; set; } 
        public  string Name { get; set; } = null!;
        public DateTimeOffset CreationDate { get; set; }
        public CategoryType Category { get; set; }
        public decimal Amount { get; set; }
        public Guid BudgetId { get; set; }
        public BudgetEntity BudgetEntity { get; set; } = null!;
    }
}
