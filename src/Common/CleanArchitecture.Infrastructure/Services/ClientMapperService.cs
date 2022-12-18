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

        public async Task<Client_Mapping> FindByMerchantWallet(string merchantWallet)
        {
            
            var item = await _dynamoDbContext.LoadAsync<Client_Mapping>(merchantWallet);

            while (item!=null)
            {
                return item;

            }

            return null;
        }


        public async Task<bool> Create(Client_Mapping model)
        {
           

            var dataPrv = await FindByMerchantWallet(model.merchantWallet);
            if (dataPrv != null)
            {
                throw new Exception($"Item in database with mobile : {model.merchantWallet} already exists\n");
            }
            var modelResponse = _dynamoDbContext.SaveAsync<Client_Mapping>(model);

            // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
            return true;
            //}
        }


        public async Task<bool> Update(Client_Mapping model)
        {


            try
            {
                var dataPrv = await FindByMerchantWallet(model.merchantWallet);
                if (dataPrv != null)
                {
                    if (dataPrv.id != model.id)
                    {
                        throw new Exception($"Item in database with mobile : {model.merchantWallet} already exists\n");
                    }
                }


                // replace the item with the updated content
                var modelResponse = _dynamoDbContext.SaveAsync<Client_Mapping>(model);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task Delete(Client_Mapping model)
        {
            var dataPrv = await FindByMerchantWallet(model.merchantWallet);
            if (dataPrv == null)
            {
               
                    throw new Exception($"Item in database with mobile : {model.merchantWallet} doesn't exists\n");
                
            }
            var modelResponse = _dynamoDbContext.DeleteAsync<Client_Mapping>(model);

        }


        public async Task<List<Client_Mapping>> GetAll(string searchText)
        {
            Task<List<Client_Mapping>> response ;

            if (!string.IsNullOrEmpty(searchText))
              {
                  response =
                      _dynamoDbContext.ScanAsync<Client_Mapping>(new[]
                              { new ScanCondition("client_name_group", ScanOperator.Equal, searchText) })
                          .GetRemainingAsync();
              }
              else
            {
                response =
                    _dynamoDbContext.ScanAsync<Client_Mapping>(default).GetRemainingAsync();
            }

          

            List<Client_Mapping> data = new List<Client_Mapping>();

            while (response.IsCompletedSuccessfully)
            {

                foreach (Client_Mapping d in response.Result)
                {
                    data.Add(d);
                }
            }

            return data;
        }


    }
}
