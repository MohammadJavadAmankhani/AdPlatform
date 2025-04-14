using AdManagementService.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdManagementService.Core.Domain.Entities
{
    public class Advertisement
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public decimal Budget { get; private set; }
        public bool IsActive { get; private set; }
        private readonly List<object> _domainEvents = new();

        public IReadOnlyList<object> DomainEvents => _domainEvents.AsReadOnly();

        public Advertisement(Guid userId, decimal budget)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Budget = budget;
            IsActive = true;
        }

        public void UpdateStatus(decimal walletBalance)
        {
            IsActive = walletBalance >= Budget;
            if (!IsActive)
                _domainEvents.Add(new AdPausedEvent(Id, UserId));
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
