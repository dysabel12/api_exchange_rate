using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_exchange_rate.Database;
using api_exchange_rate.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api_exchange_rate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeRateController : Controller
    {
        private ExchangeRateDBContext _context;

        public ExchangeRateController(ExchangeRateDBContext context)
        {
            _context = context;
        }

        // POST: ExchangeRate/GetExchangeRate
        [HttpPost]
        [Route("calculate")]
        public IActionResult GetExchangeRate(CalculateExchangeRate request)
        {
            if(request.amount_input==0 
                || request.currency_input == null
                || request.currency_input == String.Empty
                || request.currency_output ==null 
                || request.currency_output == String.Empty)
            {
                return BadRequest("No ha ingresado todos los campos obligatorios");
            }

            var select = _context.ExchangeRates.Where(x => x.currency_input == request.currency_input && x.currency_output==request.currency_output).FirstOrDefault(); ;
            if(select == null)
            {
                return BadRequest("No existe configuración para el tipo de cambio solicitado");
            }
            request.exchange_rate = select.exchange_rate;
            request.amount_output = select.exchange_rate * request.amount_input;
            return Ok(request);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult UpdateExchangeRate(ExchangeRate request)
        {
            if (request.exchange_rate == 0
                || request.currency_input == null
                || request.currency_input == String.Empty
                || request.currency_output == null
                || request.currency_output == String.Empty)
            {
                return BadRequest("No ha ingresado todos los campos obligatorios");
            }
            ExchangeRate result = (from p in _context.ExchangeRates
                             where p.currency_input == request.currency_input && p.currency_output == request.currency_output
                             select p).SingleOrDefault();

            result.exchange_rate = request.exchange_rate;

            _context.SaveChanges();
            
            return Ok(request);
        }

    }
}