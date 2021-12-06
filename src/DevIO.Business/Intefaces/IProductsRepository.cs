using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevIO.Business.Models;

namespace DevIO.Business.Intefaces
{
    public interface IProductsRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetSuppliersProducts(Guid supplierId);
        
        Task<IEnumerable<Product>> GetProductsSuppliersAsync();
        
        Task<Product> GetProductSupplierAsync(Guid productId);
    }
}