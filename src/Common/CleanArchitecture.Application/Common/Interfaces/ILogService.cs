using System.Collections.Generic;
using System.Threading.Tasks;
using BkashSNS.Domain.Entities;

namespace BkashSNS.Application.Common.Interfaces
{
    public interface ILogService
    {
        Task<List<ClientLog>> GetLogByDate(string merchantWallet, string fromDate, string toDate, string top);
        Task<bool> Insert(ClientLog log);
    }
}