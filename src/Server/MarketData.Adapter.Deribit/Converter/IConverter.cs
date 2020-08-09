namespace MarketData.Adapter.Deribit.Converter
{
    public interface IConverter
    {
         TDestination Convert<TDestination>(object source);
    }

    public interface IConverter<TDestination, TSource> : IConverter
    {
        TDestination Convert(TSource source);
    }
}