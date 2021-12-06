using System;
using System.Threading.Tasks;
using DevIO.Business.Models;

namespace DevIO.Business.Intefaces
{
    public interface IProductsService : IDisposable
    {
        Task InsertAsync(Product product);
        
        Task UpdateAsync(Product product);
        
        Task DeleteAsync(Guid productId);
    }
}