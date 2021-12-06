using System;
using System.Threading.Tasks;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Repository
{
    public class SuppliersRepository : Repository<Supplier>, ISuppliersRepository
    {
        public SuppliersRepository(MyDbContext context)
            : base(context)
        {
        }

        public async Task<Supplier> GetSupplierAddressAsync(Guid supplierId)
        {
            return await _dbContext.Suppliers.AsNoTracking()
                .Include(c => c.Address)
                .FirstOrDefaultAsync(c => c.Id == supplierId);
        }

        public async Task<Supplier> GetSupplierProductsAddressesAsync(Guid supplierId)
        {
            return await _dbContext.Suppliers.AsNoTracking()
                .Include(c => c.Products)
                .Include(c => c.Address)
                .FirstOrDefaultAsync(c => c.Id == supplierId);
        }
    }
}