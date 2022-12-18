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


        public async Task<bool> Insert(Client_Log log)
        {
         

            log.id = Guid.NewGuid().ToString();

           var response =  _dynamoDbContext.SaveAsync<Client_Log>(log);

            return true;

        }



        public async Task<List<Client_Log>> GetLogByDate(string merchantWallet, string fromDate, string toDate, string top)
        {


          var  fromDate1 = DateTime.Parse(fromDate).Date.AddDays(-1);
          var  toDate1 = DateTime.Parse(toDate).Date.AddDays(1);
          Task<List<Client_Log>> 
          response = !string.IsNullOrEmpty(merchantWallet) ? _dynamoDbContext.QueryAsync<Client_Log>(merchantWallet, QueryOperator.Between, new Object[] { fromDate1, toDate1 }).GetRemainingAsync() 
              : _dynamoDbContext.ScanAsync<Client_Log>(new[] { new ScanCondition("timestamp", ScanOperator.Between, fromDate1, toDate1) }).GetRemainingAsync();

            List<Client_Log> data = new List<Client_Log>();

            while (response.IsCompletedSuccessfully)
            {

                foreach (Client_Log d in response.Result)
                {

                    data.Add(d);

                }
            }

            return data;


        }



    }
}