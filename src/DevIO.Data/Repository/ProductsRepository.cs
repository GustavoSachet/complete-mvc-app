using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Repository
{
    public class ProductsRepository : Repository<Product>, IProductsRepository
    {
        public ProductsRepository(MyDbContext context)
            : base(context)
        {
        }

        public async Task<Product> GetProductSupplierAsync(Guid productId)
        {
            return await _dbContext.Products.AsNoTracking().Include(f => f.Supplier)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<IEnumerable<Product>> GetProductsSuppliersAsync()
        {
            return await _dbContext.Products.AsNoTracking().Include(f => f.Supplier)
                .OrderBy(p => p.Name).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetSuppliersProducts(Guid fornecedorId)
        {
            return await GetAsync(p => p.SupplierId == fornecedorId);
        }
    }
}