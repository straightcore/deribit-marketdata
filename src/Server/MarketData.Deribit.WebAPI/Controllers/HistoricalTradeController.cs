using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketData.Deribit.WebAPI.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MarketData.Deribit.WebAPI.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class HistoricalTradeController : ControllerBase
    {
        private readonly ILogger<HistoricalTradeController> _logger;
        private readonly ITradeRepository repository;

        public HistoricalTradeController(ILogger<HistoricalTradeController> logger, ITradeRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet()]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await this.repository.GetById(id));
        }
        
        [HttpGet("getByInstrumentName")]
        public async Task<IActionResult> GetByInstrumentName(string instrument)
        {
            return Ok(await this.repository.GetByInstrumentName(instrument));
        }
    }
}
