using System;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Internal;
using Amazon.Runtime;
using BkashSNS.Application.Common.Interfaces;
using BkashSNS.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BkashSNS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)//, IWebHostEnvironment environment)
        {

            var credential = new BasicAWSCredentials(configuration["AWS_CREDENTIAL:accessKey"], configuration["AWS_CREDENTIAL:secretKey"]);
            var config = new AmazonDynamoDBConfig()
            {
                RegionEndpoint = RegionEndpoint.APNortheast1
            };
            var client = new AmazonDynamoDBClient(credential, config);
            services.AddSingleton<IAmazonDynamoDB>(client);
            services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IClientMappingService, ClientMappingService>();



            return services;
        }
    }
}
