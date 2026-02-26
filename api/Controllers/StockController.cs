using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context; // field to hold the database context, which will be injected via the constructor
        public StockController(ApplicationDBContext context) // constructor that takes in the database context and assigns it to the field
        {
            _context = context;
        }
        [HttpGet] // GET: api/stock (get = read)
        public IActionResult GetAll()
        {
            var stocks = _context.Stocks.ToList() //reference to table of stocks, then execute query to get stocks as a list
             .Select(s => s.ToStockDTO()); // map to DTOs

            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var stock = _context.Stocks.Find(id);

            if(stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDTO());
        }
    }
}