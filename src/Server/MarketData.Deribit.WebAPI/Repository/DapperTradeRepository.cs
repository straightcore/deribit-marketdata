using MarketData.Deribit.Models;
using System;
using System.Data;

namespace MarketData.Deribit.WebAPI.Repository
{
    public class DapperTradeRepository
    {
        private readonly IDbConnection connection;
        public DapperTradeRepository(IDbConnection connection)
        {
            this.connection = connection ?? throw new System.ArgumentNullException(nameof(connection));

        }

        public Trade GetById(string id)
        {
            return this.connection.Query<Trade>("Select * from ")
        }
        public Trade GetById(string intrumentName, DateTime tradeDate)
        {
        
        }
    }
}