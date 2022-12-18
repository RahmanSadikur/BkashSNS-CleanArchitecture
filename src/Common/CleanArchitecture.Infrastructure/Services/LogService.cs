using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using BkashSNS.Application.Common.Interfaces;
using BkashSNS.Domain.Entities;

namespace BkashSNS.Infrastructure.Services
{
    public class LogService:ILogService
    {
        //dynamo db 

        private readonly IDynamoDBContext _dynamoDbContext;


        public LogService(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;

            
        }


        public async Task<bool> Insert(ClientLog log)
        {
         

            log.Id = Guid.NewGuid().ToString();

           var response =  _dynamoDbContext.SaveAsync<ClientLog>(log);

            return true;

        }



        public async Task<List<ClientLog>> GetLogByDate(string merchantWallet, string fromDate, string toDate, string top)
        {


          var  fromDate1 = DateTime.Parse(fromDate).Date.AddDays(-1);
          var  toDate1 = DateTime.Parse(toDate).Date.AddDays(1);
          Task<List<ClientLog>> 
          response = !string.IsNullOrEmpty(merchantWallet) ? _dynamoDbContext.QueryAsync<ClientLog>(merchantWallet, QueryOperator.Between, new Object[] { fromDate1, toDate1 }).GetRemainingAsync() 
              : _dynamoDbContext.ScanAsync<ClientLog>(new[] { new ScanCondition("Timestamp", ScanOperator.Between, fromDate1, toDate1) }).GetRemainingAsync();

            List<ClientLog> data = new List<ClientLog>();

            while (response.IsCompletedSuccessfully)
            {

                foreach (ClientLog d in response.Result)
                {

                    data.Add(d);

                }
            }

            return data;


        }



    }
}