using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _dBContext;
        public StockRepository(ApplicationDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _dBContext.Stocks.AddAsync(stock);
            await _dBContext.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stock = await _dBContext.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if(stock == null) 
            {
                return null;
            }

            _dBContext.Stocks.Remove(stock);
            await _dBContext.SaveChangesAsync();
            return stock;
        }

        public async Task<List<Stock>> GetAllStocksAsync()
        {
            return await _dBContext.Stocks.Include(c => c.Comments).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _dBContext.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, Stock stock)
        {
            var existingStock = await _dBContext.Stocks.FindAsync(id);

            if (existingStock == null) 
            {
                return null;
            }

            existingStock.Symbol = stock.Symbol;
            existingStock.CompanyName = stock.CompanyName;
            existingStock.PurchasePrice = stock.PurchasePrice;
            existingStock.LastDiv = stock.LastDiv;
            existingStock.Industry = stock.Industry;
            existingStock.MarketCap = stock.MarketCap;

            await _dBContext.SaveChangesAsync();
            
            return existingStock;
        }
    }
}