using AdDeliveryService.Core.Domain.Entities;
using AdDeliveryService.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdDeliveryService.Application.Queries
{
    public class GetActiveAdsQuery : IRequest<List<AdDelivery>>
    {
    }

    public class GetActiveAdsQueryHandler : IRequestHandler<GetActiveAdsQuery, List<AdDelivery>>
    {
        private readonly IAdDeliveryRepository _adRepo;

        public GetActiveAdsQueryHandler(IAdDeliveryRepository adRepo)
        {
            _adRepo = adRepo;
        }

        public async Task<List<AdDelivery>> Handle(GetActiveAdsQuery request, CancellationToken cancellationToken)
        {
            return await _adRepo.GetActiveAdsAsync();
        }
    }
}
