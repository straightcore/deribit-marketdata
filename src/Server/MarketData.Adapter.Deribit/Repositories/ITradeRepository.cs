using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketData.Adapter.Deribit.Repositories
{
    public interface ITradeRepository<T>
    {
        Task Insert(T trade);
        Task Insert(IEnumerable<T> trades);
        Task Save();
    }
}