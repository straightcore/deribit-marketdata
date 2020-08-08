using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;

namespace MarketData.Adapter.Deribit.Configuration.Validators
{
    public class ServiceConfigValidator : AbstractValidator<ServiceConfig>
    {
        public ServiceConfigValidator()
        {
            RuleFor(config => config.Url).NotNull().NotNull().WithMessage("Deribit URL is required to start the service");
            RuleFor(config => config.FetchInterval).NotNull().NotNull().GreaterThan(0).WithMessage("Interval must be greater than 0");
            RuleFor(config => config.Authentification.ClientId).NotEmpty().NotEmpty().When(config => config.Authentification != null);
            RuleFor(config => config.Authentification.ClientSecret).NotEmpty().NotEmpty().When(config => config.Authentification != null);
        }

    }
}