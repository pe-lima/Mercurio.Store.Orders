﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IOrderRepository Order { get; }

        Task<int> SaveChangesAsync();
    }
}
