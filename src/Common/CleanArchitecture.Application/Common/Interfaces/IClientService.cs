using BkashSNS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BkashSNS.Application.Common.Interfaces
{
    public interface IClientService
    {
        Task Insert(Message message);
        Task<Message> GetLastPaymentInfo(string merchantWallet, string counterNo);
        Task<List<Message>> GetPaymentInfoByDate(string merchantWallet, string fromDate, string toDate, string top);
    }
}