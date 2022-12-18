using BkashSNS.Infrastructure.Services.Helper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using BkashSNS.Domain.Entities;
using Amazon.DynamoDBv2.DocumentModel;
using BkashSNS.Application.Common.Interfaces;

namespace BkashSNS.Infrastructure.Services
{
    public class ClientService:IClientService
    {
        private readonly IDynamoDBContext _dynamoDbContext;


        //DataProvider dataProvider;
        public ClientService(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;

        }

        public async Task Insert(PaymentRecord message)
        {
            

            

            message.Id = Guid.NewGuid().ToString();


            var response = _dynamoDbContext.SaveAsync<PaymentRecord>(message);



        }

        public async Task<PaymentRecord> GetLastPaymentInfo(string merchantWallet, string counterNo)
        {


           var response= _dynamoDbContext.ScanAsync<PaymentRecord>(new[] { new ScanCondition("MerchantWallet", ScanOperator.Equal, merchantWallet ),
               new ScanCondition("TerminalId", ScanOperator.Equal, counterNo) }).GetRemainingAsync();


            List<PaymentRecord> data = new List<PaymentRecord>();

            while (response.IsCompletedSuccessfully)
            {
                foreach (PaymentRecord d in response.Result)
                {
                    return d;
                   
                }
            }

            return null;


        }


        public async Task<List<PaymentRecord>> GetPaymentInfoByDate(string merchantWallet, string fromDate, string toDate, string top)
        {


            var fromDate1 = DateTime.Parse(fromDate).Date.AddDays(-1).ToString("yyyy-MM-dd");
            var toDate1 = DateTime.Parse(toDate).AddDays(1).ToString("yyyy-MM-dd");

            var response =  _dynamoDbContext
                    .QueryAsync<PaymentRecord>(merchantWallet, QueryOperator.Between, new Object[] { fromDate1, toDate1 })
                    .GetRemainingAsync();

             List <PaymentRecord> data = new List<PaymentRecord>();

            while (response.IsCompletedSuccessfully)
            {

                foreach (PaymentRecord d in response.Result)
                {

                    data.Add(d);

                }
            }

            return data;


        }



    }
}
