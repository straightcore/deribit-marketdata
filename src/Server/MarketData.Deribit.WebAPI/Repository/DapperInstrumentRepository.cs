using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace MarketData.Deribit.WebAPI.Repository
{
    public interface IInstrumentRepository : IDisposable
    {
        Task<IEnumerable<InstrumentDto>> Get();
    }

    public class DapperInstrumentRepository : IInstrumentRepository
    {
        private IDbConnection connection;
        private bool isDisposedValue;

        public DapperInstrumentRepository(IDbConnection connection)
        {
            this.connection = connection ?? throw new System.ArgumentNullException(nameof(connection));
        }

        public Task<IEnumerable<InstrumentDto>> Get()
        {
            CheckIfDisposed();
            return this.connection.QueryAsync<InstrumentDto>("Select InstrumentName from Trades");
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