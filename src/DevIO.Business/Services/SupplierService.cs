using System;
using System.Linq;
using System.Threading.Tasks;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;

namespace DevIO.Business.Services
{
    public class SupplierService : BaseService, ISupplierService
    {
        private readonly ISuppliersRepository _suppliersRepository;
        private readonly IAddressesRepository _addressesRepository;

        public SupplierService(
            ISuppliersRepository supplierRepository,
            IAddressesRepository addressRepository,
            INotifier notifier)
            : base(notifier)
        {
            _suppliersRepository = supplierRepository;
            _addressesRepository = addressRepository;
        }

        public async Task InsertAsync(Supplier supplier)
        {
            if (!ExecuteValidation(new SupplierValidations(), supplier) || !ExecuteValidation(new AddressValidations(), supplier.Address))
                return;

            if (_suppliersRepository.GetAsync(f => f.Document == supplier.Document).Result.Any())
            {
                Notify("Já existe um fornecedor com este documento infomado.");
                return;
            }

            await _suppliersRepository.InsertAsync(supplier);
        }

        public async Task UpdateAsync(Supplier supplier)
        {
            if (!ExecuteValidation(new SupplierValidations(), supplier))
                return;

            if (_suppliersRepository.GetAsync(f => f.Document == supplier.Document && f.Id != supplier.Id).Result.Any())
            {
                Notify("Já existe um fornecedor com este documento infomado.");
                return;
            }

            await _suppliersRepository.UpdateAsync(supplier);
        }

        public async Task UpdateAddressAsync(Address address)
        {
            if (!ExecuteValidation(new AddressValidations(), address))
                return;

            await _addressesRepository.UpdateAsync(address);
        }

        public async Task DeleteAsync(Guid supplierId)
        {
            if (supplierId == Guid.Empty)
                throw new ArgumentNullException(nameof(supplierId));

            var supplier = await _suppliersRepository.GetSupplierProductsAddressesAsync(supplierId);

            if (supplier is null)
                Notify();

            if (supplier.Products.Any())
            {
                Notify("O fornecedor possui produtos cadastrados!");
                return;
            }

            var supplierAddress = await _addressesRepository.GetSupplierAddressAsync(supplierId);

            if (supplierAddress != null)
            {
                await _addressesRepository.DeleteAsync(supplierAddress.Id);
            }

            await _suppliersRepository.DeleteAsync(supplierId);
        }

        public void Dispose()
        {
            _suppliersRepository?.Dispose();
            _addressesRepository?.Dispose();
        }
    }
}