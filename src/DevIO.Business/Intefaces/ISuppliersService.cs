using System;
using System.Threading.Tasks;
using DevIO.Business.Models;

namespace DevIO.Business.Intefaces
{
    public interface ISupplierService : IDisposable
    {
        Task InsertAsync(Supplier fornecedor);
        
        Task UpdateAsync(Supplier fornecedor);
        
        Task DeleteAsync(Guid id);

        Task UpdateAddressAsync(Address endereco);
    }
}