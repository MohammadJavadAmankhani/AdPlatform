using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdManagementService.Core.Domain.Events
{
    public class AdPausedEvent
    {
        public string Type => nameof(AdPausedEvent);
        public Guid AdId { get; set; }
        public Guid UserId { get; set; }

        public AdPausedEvent(Guid adId, Guid userId)
        {
            AdId = adId;
            UserId = userId;
        }
    }
}
