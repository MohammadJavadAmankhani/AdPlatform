﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletService.Core.Interfaces
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T @event);
    }
}
