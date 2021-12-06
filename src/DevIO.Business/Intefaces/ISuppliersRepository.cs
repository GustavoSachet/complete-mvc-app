using System;
using System.Threading.Tasks;
using DevIO.Business.Models;

namespace DevIO.Business.Intefaces
{
    public interface ISuppliersRepository : IRepository<Supplier>
    {
        Task<Supplier> GetSupplierAddressAsync(Guid supplierId);
        Task<Supplier> GetSupplierProductsAddressesAsync(Guid supplierId);
    }
}