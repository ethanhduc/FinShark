using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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

        [HttpPost] 
        public IActionResult Create([FromBody] CreateStockRequestDTO createStockDTO) //automatically deserialize JSON body into CreateStockRequestDTO object
        {
            var stockModel = createStockDTO.ToStockFromCreateDTO(); // map from DTO to model
            _context.Stocks.Add(stockModel); // add to database context
            _context.SaveChanges(); // save changes to database (id generated here)
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDTO()); // return 201 with location header pointing to new resource
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateDTO)
        {
            var stockModel = _context.Stocks.FirstOrDefault(x => x.Id == id); // find stock by id

            if(stockModel == null)
            {
                return NotFound();
            }

            stockModel.Symbol = updateDTO.Symbol;
            stockModel.CompanyName = updateDTO.CompanyName;
            stockModel.Purchase = updateDTO.Purchase;
            stockModel.LastDiv = updateDTO.LastDiv;
            stockModel.Industry = updateDTO.Industry;
            stockModel.MarketCap = updateDTO.MarketCap;

            _context.SaveChanges();

            return Ok(stockModel.ToStockDTO());
        }
    }
}