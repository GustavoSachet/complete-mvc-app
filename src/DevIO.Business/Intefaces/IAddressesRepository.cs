using System;
using System.Threading.Tasks;
using DevIO.Business.Models;

namespace DevIO.Business.Intefaces
{
    public interface IAddressesRepository : IRepository<Address>
    {
        Task<Address> GetSupplierAddressAsync(Guid supplierId);
    }
}