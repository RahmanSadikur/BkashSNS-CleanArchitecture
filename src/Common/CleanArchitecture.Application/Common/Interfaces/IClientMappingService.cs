using System.Collections.Generic;
using System.Threading.Tasks;
using BkashSNS.Domain.Entities;

namespace BkashSNS.Application.Common.Interfaces
{
    public interface IClientMappingService
    {
        Task<ClientMapping> FindByMerchantWallet(string merchantWallet);
        Task<bool> Create(ClientMapping model);
        Task<bool> Update(ClientMapping model);
        Task Delete(ClientMapping model);
        Task<List<ClientMapping>> GetAll(string searchText);
    }
}