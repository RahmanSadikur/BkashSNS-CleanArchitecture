using BkashSNS.Domain.Entities;
using BkashSNS.Infrastructure.Services.Helper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using BkashSNS.Application.Common.Interfaces;
using Amazon.DynamoDBv2.DocumentModel;

namespace BkashSNS.Infrastructure.Services
{
    public class ClientMappingService: IClientMappingService
    {
        private readonly IDynamoDBContext _dynamoDbContext;




        public ClientMappingService(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;


        }

        public async Task<ClientMapping> FindByMerchantWallet(string merchantWallet)
        {
            
            var item = await _dynamoDbContext.LoadAsync<ClientMapping>(merchantWallet);

            while (item!=null)
            {
                return item;

            }

            return null;
        }


        public async Task<bool> Create(ClientMapping model)
        {
           

            var dataPrv = await FindByMerchantWallet(model.MerchantWallet);
            if (dataPrv != null)
            {
                throw new Exception($"Item in database with mobile : {model.MerchantWallet} already exists\n");
            }
            var modelResponse = _dynamoDbContext.SaveAsync<ClientMapping>(model);

            // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
            return true;
            //}
        }


        public async Task<bool> Update(ClientMapping model)
        {


            try
            {
                var dataPrv = await FindByMerchantWallet(model.MerchantWallet);
                if (dataPrv != null)
                {
                    if (dataPrv.Id != model.Id)
                    {
                        throw new Exception($"Item in database with mobile : {model.MerchantWallet} already exists\n");
                    }
                }


                // replace the item with the updated content
                var modelResponse = _dynamoDbContext.SaveAsync<ClientMapping>(model);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task Delete(ClientMapping model)
        {
            var dataPrv = await FindByMerchantWallet(model.MerchantWallet);
            if (dataPrv == null)
            {
               
                    throw new Exception($"Item in database with mobile : {model.MerchantWallet} doesn't exists\n");
                
            }
            var modelResponse = _dynamoDbContext.DeleteAsync<ClientMapping>(model);

        }


        public async Task<List<ClientMapping>> GetAll(string searchText)
        {
            Task<List<ClientMapping>> response ;

            if (!string.IsNullOrEmpty(searchText))
              {
                  response =
                      _dynamoDbContext.ScanAsync<ClientMapping>(new[]
                              { new ScanCondition("ClientNameGroup", ScanOperator.Equal, searchText) })
                          .GetRemainingAsync();
              }
              else
            {
                response =
                    _dynamoDbContext.ScanAsync<ClientMapping>(default).GetRemainingAsync();
            }

          

            List<ClientMapping> data = new List<ClientMapping>();

            while (response.IsCompletedSuccessfully)
            {

                foreach (ClientMapping d in response.Result)
                {
                    data.Add(d);
                }
            }

            return data;
        }


    }
}
