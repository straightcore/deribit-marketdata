using FluentValidation;

namespace MarketData.Adapter.Deribit.Configuration.Validators
{
    public class InstrumentConfigurationValidator : AbstractValidator<Instrument>
    {
        public InstrumentConfigurationValidator()
        {
            RuleFor(config => config.Currency).NotNull();
            RuleFor(config => config.Kind).NotNull();
        }
    }
}