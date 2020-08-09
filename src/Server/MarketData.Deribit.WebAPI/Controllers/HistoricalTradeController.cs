using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public HistoricalTradeController(ILogger<HistoricalTradeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [EnableCors]
        
        public IEnumerable<string> Get()
        {
            return new string[] { "value one from api version", "One"};
        }
    }
}
