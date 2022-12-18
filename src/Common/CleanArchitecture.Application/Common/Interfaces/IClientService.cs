using BkashSNS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BkashSNS.Application.Common.Interfaces
{
    public interface IClientService
    {
        Task Insert(PaymentRecord message);
        Task<PaymentRecord> GetLastPaymentInfo(string merchantWallet, string counterNo);
        Task<List<PaymentRecord>> GetPaymentInfoByDate(string merchantWallet, string fromDate, string toDate, string top);
    }
}