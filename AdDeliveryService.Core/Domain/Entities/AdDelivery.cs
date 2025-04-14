using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdDeliveryService.Core.Domain.Entities
{
    public class AdDelivery
    {
        public Guid Id { get; private set; }
        public Guid AdId { get; private set; }
        public string Content { get; private set; }
        public bool IsActive { get; private set; }

        public AdDelivery(Guid adId, string content)
        {
            Id = Guid.NewGuid();
            AdId = adId;
            Content = content;
            IsActive = true;
        }

        public void Pause()
        {
            IsActive = false;
        }
    }
}
