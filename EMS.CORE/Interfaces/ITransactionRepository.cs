﻿using EMS.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.CORE.Interfaces
{
    public interface ITransactionRepository
    {
        Task<TransactionEntity> AddTransactionAsync(TransactionEntity entity);
        Task<IEnumerable<TransactionEntity>> GetTransactionsByBudgetIdAsync(Guid id);
    }
}