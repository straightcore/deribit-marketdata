using MarketData.Deribit.Models;
using System;
using System.Data;
using Dapper;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MarketData.Deribit.WebAPI.Repository
{
    public interface ITradeRepository : IDisposable
    {
        Task<Trade> GetById(string id);
        Task<IEnumerable<Trade>> GetByInstrumentName(string intrumentName);
    }

    public class DapperTradeRepository : ITradeRepository
    {
        private IDbConnection connection;
        private bool isDisposedValue;

        public DapperTradeRepository(IDbConnection connection)
        {
            this.connection = connection ?? throw new System.ArgumentNullException(nameof(connection));

        }

        public Task<Trade> GetById(string id)
        {
            CheckIfDisposed();
            return this.connection.QueryFirstAsync<Trade>("Select * from Trades where id=@identifier", new { identifier = id });
        }

        public Task<IEnumerable<Trade>> GetByInstrumentName(string intrumentName)
        {
            CheckIfDisposed();
            return this.connection.QueryAsync<Trade>("Select * from Trades where InstrumentName=@name", new { name = intrumentName });
        }

        private void CheckIfDisposed()
        {
            if(isDisposedValue)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposedValue)
            {
                if (disposing)
                {
                    connection?.Dispose();
                }
                connection = null;
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                isDisposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~DapperInstrumentRepository()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

    }
}