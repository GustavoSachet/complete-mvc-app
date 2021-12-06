using System;
using System.Threading.Tasks;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Repository
{
    public class AddressesRepository : Repository<Address>, IAddressesRepository
    {
        public AddressesRepository(MyDbContext context)
            : base(context)
        {
        }

        public async Task<Address> GetSupplierAddressAsync(Guid supplierId)
        {
            return await _dbContext.Addresses.AsNoTracking()
                .FirstOrDefaultAsync(f => f.SupplierId == supplierId);
        }
    }
}