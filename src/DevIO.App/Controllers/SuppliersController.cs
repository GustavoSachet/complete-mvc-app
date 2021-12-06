using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevIO.App.Extensions;
using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;

namespace DevIO.App.Controllers
{
    [Authorize]
    public class SuppliersController : BaseController
    {
        private readonly ISuppliersRepository _suppliersRepository;
        private readonly ISupplierService _suppliersService;
        private readonly IMapper _mapper;

        public SuppliersController(
            ISuppliersRepository suppliersRepository,
            ISupplierService suppliersService,
            IMapper mapper,
            INotifier notifier)
            : base(notifier)
        {
            _suppliersRepository = suppliersRepository;
            _mapper = mapper;
            _suppliersService = suppliersService;
        }

        [AllowAnonymous]
        [Route("lista-de-fornecedores")]
        public async Task<IActionResult> Index()
        {
            var suppliers = await _suppliersRepository.GetAllAsync();

            return View(_mapper.Map<IEnumerable<SupplierViewModel>>(suppliers));
        }

        [AllowAnonymous]
        [Route("dados-do-fornecedor/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var supplierViewModel = await GetSupplierAddress(id);

            if (supplierViewModel is null)
                return NotFound();

            return View(supplierViewModel);
        }

        [Route("novo-fornecedor")]
        [ClaimsAuthorize("Supplier", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("novo-fornecedor")]
        [ClaimsAuthorize("Supplier", "Create")]
        public async Task<IActionResult> Create(SupplierViewModel supplierViewModel)
        {
            if (!ModelState.IsValid)
                return View(supplierViewModel);

            var supplier = _mapper.Map<Supplier>(supplierViewModel);

            await _suppliersService.InsertAsync(supplier);

            if (!ValidOperation())
                return View(supplierViewModel);

            return RedirectToAction("Index");
        }

        [Route("editar-fornecedor/{id:guid}")]
        [ClaimsAuthorize("Supplier", "Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var supplierViewModel = await GetSupplierProductsAddress(id);

            if (supplierViewModel is null)
                return NotFound();

            return View(supplierViewModel);
        }

        [HttpPost]
        [Route("editar-fornecedor/{id:guid}")]
        [ClaimsAuthorize("Supplier", "Edit")]
        public async Task<IActionResult> Edit(Guid id, SupplierViewModel supplierViewModel)
        {
            if (id != supplierViewModel.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(supplierViewModel);

            var supplier = _mapper.Map<Supplier>(supplierViewModel);

            await _suppliersService.UpdateAsync(supplier);

            if (!ValidOperation())
                return View(await GetSupplierProductsAddress(id));

            return RedirectToAction("Index");
        }

        [Route("excluir-fornecedor/{id:guid}")]
        [ClaimsAuthorize("Supplier", "Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var supplierViewModel = await GetSupplierAddress(id);

            if (supplierViewModel is null)
                return NotFound();

            return View(supplierViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [Route("excluir-fornecedor/{id:guid}")]
        [ClaimsAuthorize("Supplier", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var supplier = await GetSupplierAddress(id);

            if (supplier is null)
                return NotFound();

            await _suppliersService.DeleteAsync(id);

            if (!ValidOperation())
                return View(supplier);

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [Route("obter-endereco-fornecedor/{id:guid}")]
        public async Task<IActionResult> GetAddress(Guid id)
        {
            var supplier = await GetSupplierAddress(id);

            if (supplier is null)
                return NotFound();

            return PartialView("_AddressDetails", supplier);
        }

        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        [ClaimsAuthorize("Supplier", "Edit")]
        public async Task<IActionResult> UpdateAddress(Guid id)
        {
            var supplier = await GetSupplierAddress(id);

            if (supplier is null)
                return NotFound();

            return PartialView("_UpdateAddress", new SupplierViewModel { Address = supplier.Address });
        }

        [HttpPost]
        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        [ClaimsAuthorize("Supplier", "Edit")]
        public async Task<IActionResult> UpdateAddress(SupplierViewModel supplierViewModel)
        {
            ModelState.Remove("Name");
            ModelState.Remove("Document");

            if (!ModelState.IsValid)
                return PartialView("_UpdateAddress", supplierViewModel);

            await _suppliersService.UpdateAddressAsync(_mapper.Map<Address>(supplierViewModel.Address));

            if (!ValidOperation())
                return PartialView("_UpdateAddress", supplierViewModel);

            var url = Url.Action("GetAddress", "Suppliers", new { id = supplierViewModel.Address.SupplierId });

            return Json(new { success = true, url });
        }

        private async Task<SupplierViewModel> GetSupplierAddress(Guid supplierId)
        {
            var supplier = await _suppliersRepository.GetSupplierAddressAsync(supplierId);

            return _mapper.Map<SupplierViewModel>(supplier);
        }

        private async Task<SupplierViewModel> GetSupplierProductsAddress(Guid suplierId)
        {
            var supplier = await _suppliersRepository.GetSupplierProductsAddressesAsync(suplierId);

            return _mapper.Map<SupplierViewModel>(supplier);
        }
    }
}