using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel); // add to database context
            await _context.SaveChangesAsync(); // save changes to database (id generated here)
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if(stockModel == null)
            {
                return null;
            }

            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.FindAsync(id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO updateStockDTO)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if(existingStock == null)
            {
                return null;
            }

            existingStock.Symbol = updateStockDTO.Symbol;
            existingStock.CompanyName = updateStockDTO.CompanyName;
            existingStock.Purchase = updateStockDTO.Purchase;
            existingStock.LastDiv = updateStockDTO.LastDiv;
            existingStock.Industry = updateStockDTO.Industry;
            existingStock.MarketCap = updateStockDTO.MarketCap;

            await _context.SaveChangesAsync();
            return existingStock;
        }
    }
}