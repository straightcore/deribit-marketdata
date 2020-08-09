using System;
using System.Threading.Tasks;
using MarketData.Deribit.WebAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MarketData.Deribit.WebAPI.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class InstrumentController: ControllerBase
    {
        private readonly ILogger<InstrumentController> _logger;
        private readonly IInstrumentRepository repository;

        public InstrumentController(ILogger<InstrumentController> logger, IInstrumentRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await this.repository.Get());
        }

    }
}