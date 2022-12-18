using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BkashSNS.Infrastructure.Services.Helper
{
    public class StaticData
    {
        public static string GetDbConnection { get; set; }


        // The Azure Cosmos DB endpoint for running this sample.
        public static string EndpointUri { get; set; }

        // The primary key for the Azure Cosmos account.
        public static string PrimaryKey { get; set; }

        public static string databaseId { get; set; }

        public static string tokenStore = @"eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiQWRtaW4iLCJJc3N1ZXIiOiJGYXpsZSBJbWFtdWwgS2FyaW0iLCJVc2VybmFtZSI6ImFkbWluIiwiZXhwIjoxODc1MDcxNDQyLCJpYXQiOjE2MjI2MTA2NDJ9.RcKbWyRM73oMwzkdph2PeyUSGpKrwkCa1p-dOlhx3TY";

        public static bool IsAuthorized(string authorizationHeader)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return false;
            }
            if (authorizationHeader.Split(" ").Count() == 0)
            {
                return false;
            }

            if (authorizationHeader.Split(" ")[0] != "Bearer")
            {
                return false;
            }

            if (authorizationHeader.Split(" ")[1].Trim() != tokenStore)
            {
                return false;
            }

            return true;
        }

    }
}
