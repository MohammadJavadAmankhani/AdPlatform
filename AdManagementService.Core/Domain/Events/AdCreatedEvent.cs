using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdManagementService.Core.Domain.Events
{
    public class AdCreatedEvent
    {
        public string Type => nameof(AdCreatedEvent);
        public Guid AdId { get; set; }
        public Guid UserId { get; set; }
        public decimal Budget { get; set; }
        public bool IsActive { get; set; }

        public AdCreatedEvent(Guid adId, Guid userId, decimal budget, bool isActive)
        {
            AdId = adId;
            UserId = userId;
            Budget = budget;
            IsActive = isActive;
        }
    }
}
