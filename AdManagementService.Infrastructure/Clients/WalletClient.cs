using AdManagementService.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WalletService.Core.Domain.Entities;

namespace AdManagementService.Infrastructure.Clients
{
    public class WalletClient : IWalletClient
    {
        private readonly HttpClient _httpClient;

        public WalletClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5037/");
        }

        public async Task<decimal> GetBalanceAsync(Guid userId, AccountType accountType)
        {
            var response = await _httpClient.GetAsync($"/api/wallet/balance?userId={userId}&accountType={accountType}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<decimal>();
        }
    }
}
