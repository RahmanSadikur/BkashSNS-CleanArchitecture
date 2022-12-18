using System.Collections.Generic;
using System.Threading.Tasks;
using BkashSNS.Domain.Entities;

namespace BkashSNS.Application.Common.Interfaces
{
    public interface ILogService
    {
        Task<List<Client_Log>> GetLogByDate(string merchantWallet, string fromDate, string toDate, string top);
        Task<bool> Insert(Client_Log log);
    }
}