using AdDeliveryService.Core.Domain.Entities;
using AdDeliveryService.Core.Interfaces;
using Microsoft.Azure.Cosmos;

namespace AdDeliveryService.Infrastructure.Data
{
    public class CosmosDbRepository : IAdDeliveryRepository
    {
        private readonly Container _container;

        public CosmosDbRepository(CosmosClient client)
        {
            _container = client.GetContainer("AdsDb", "Ads");
        }

        public async Task<List<AdDelivery>> GetActiveAdsAsync()
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.IsActive = @isActive")
                .WithParameter("@isActive", true);
            var iterator = _container.GetItemQueryIterator<AdDelivery>(query);
            var results = new List<AdDelivery>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public async Task<AdDelivery> GetByAdIdAsync(Guid adId)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.AdId = @adId")
                .WithParameter("@adId", adId.ToString());
            var iterator = _container.GetItemQueryIterator<AdDelivery>(query);
            var response = await iterator.ReadNextAsync();
            return response.FirstOrDefault();
        }

        public async Task SaveAsync(AdDelivery ad)
        {
            await _container.UpsertItemAsync(ad, new PartitionKey(ad.AdId.ToString()));
        }
    }
}
