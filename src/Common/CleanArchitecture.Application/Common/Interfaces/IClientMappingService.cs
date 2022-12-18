using System.Collections.Generic;
using System.Threading.Tasks;
using BkashSNS.Domain.Entities;

namespace BkashSNS.Application.Common.Interfaces
{
    public interface IClientMappingService
    {
        Task<Client_Mapping> FindByMerchantWallet(string merchantWallet);
        Task<bool> Create(Client_Mapping model);
        Task<bool> Update(Client_Mapping model);
        Task Delete(Client_Mapping model);
        Task<List<Client_Mapping>> GetAll(string searchText);
    }
}