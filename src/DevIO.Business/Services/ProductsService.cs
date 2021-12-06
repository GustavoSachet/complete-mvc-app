using System;
using System.Threading.Tasks;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;

namespace DevIO.Business.Services
{
    public class ProductsService : BaseService, IProductsService
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsService(IProductsRepository productsRepository, INotifier notifier)
            : base(notifier)
        {
            _productsRepository = productsRepository;
        }

        public async Task InsertAsync(Product product)
        {
            if (!ExecuteValidation(new ProductValidations(), product)) return;

            await _productsRepository.InsertAsync(product);
        }

        public async Task UpdateAsync(Product product)
        {
            if (!ExecuteValidation(new ProductValidations(), product)) return;

            await _productsRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(Guid productId)
        {
            await _productsRepository.DeleteAsync(productId);
        }

        public void Dispose()
        {
            _productsRepository?.Dispose();
        }
    }
}