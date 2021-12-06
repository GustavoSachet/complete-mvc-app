using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using DevIO.App.Extensions;
using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace DevIO.App.Controllers
{
    [Authorize]
    public class ProductsController : BaseController
    {
        private readonly IProductsRepository _productsRepository;
        private readonly ISuppliersRepository _suppliersRepository;
        private readonly IProductsService _productsService;
        private readonly IMapper _mapper;

        public ProductsController(
            IProductsRepository productsRepository,
            ISuppliersRepository suppliersRepository,
            IProductsService productsService,
            IMapper mapper,
            INotifier notifier)
            : base(notifier)
        {
            _productsRepository = productsRepository;
            _suppliersRepository = suppliersRepository;
            _productsService = productsService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("lista-de-produtos")]
        public async Task<IActionResult> Index()
        {
            var products = await _productsRepository.GetProductsSuppliersAsync();

            return View(_mapper.Map<IEnumerable<ProductViewModel>>(products));
        }

        [AllowAnonymous]
        [Route("dados-do-produto/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var produtoViewModel = await GetProductAsync(id);

            if (produtoViewModel is null)
                return NotFound();

            return View(produtoViewModel);
        }

        [Route("novo-produto")]
        [ClaimsAuthorize("Product","Create")]
        public async Task<IActionResult> Create()
        {
            var produtoViewModel = await PopulateSuppliersAsync(new ProductViewModel());

            return View(produtoViewModel);
        }

        [HttpPost]
        [Route("novo-produto")]
        [ClaimsAuthorize("Product", "Create")]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            productViewModel = await PopulateSuppliersAsync(productViewModel);

            if (!ModelState.IsValid)
                return View(productViewModel);

            var imgPrefix = Guid.NewGuid() + "_";

            var uploadArchive = await UploadArchiveAsync(productViewModel.ImageUpload, imgPrefix);

            if (!uploadArchive)
                return View(productViewModel);

            productViewModel.Image = imgPrefix + productViewModel.ImageUpload.FileName;

            await _productsService.InsertAsync(_mapper.Map<Product>(productViewModel));

            if (!ValidOperation())
                return View(productViewModel);

            return RedirectToAction("Index");
        }

        [Route("editar-produto/{id:guid}")]
        [ClaimsAuthorize("Product", "Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var productViewModel = await GetProductAsync(id);

            if (productViewModel is null)
                return NotFound();

            return View(productViewModel);
        }

        [HttpPost]
        [Route("editar-produto/{id:guid}")]
        [ClaimsAuthorize("Product", "Edit")]
        public async Task<IActionResult> Edit(Guid id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
                return NotFound();

            var productUpdate = await GetProductAsync(id);

            productViewModel.Supplier = productUpdate.Supplier;
            productViewModel.Image = productUpdate.Image;

            if (!ModelState.IsValid)
                return View(productViewModel);

            if (productViewModel.ImageUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "_";

                var uploadArchive = await UploadArchiveAsync(productViewModel.ImageUpload, imgPrefixo);

                if (!uploadArchive)
                    return View(productViewModel);

                productUpdate.Image = imgPrefixo + productViewModel.ImageUpload.FileName;
            }

            productUpdate.Name = productViewModel.Name;
            productUpdate.Description = productViewModel.Description;
            productUpdate.Price = productViewModel.Price;
            productUpdate.Active = productViewModel.Active;

            await _productsService.UpdateAsync(_mapper.Map<Product>(productUpdate));

            if (!ValidOperation())
                return View(productViewModel);

            return RedirectToAction("Index");
        }

        [Route("excluir-produto/{id:guid}")]
        [ClaimsAuthorize("Product", "Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await GetProductAsync(id);

            if (product is null)
                return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [Route("excluir-produto/{id:guid}")]
        [ClaimsAuthorize("Product", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await GetProductAsync(id);

            if (product is null)
                return NotFound();

            await _productsService.DeleteAsync(id);

            if (!ValidOperation())
                return View(product);

            TempData["Sucesso"] = "Produto excluido com sucesso!";

            return RedirectToAction("Index");
        }

        private async Task<ProductViewModel> GetProductAsync(Guid productId)
        {
            var product = await _productsRepository.GetProductSupplierAsync(productId);

            var productViewModel = _mapper.Map<ProductViewModel>(product);

            var suppliers = await _suppliersRepository.GetAllAsync();

            productViewModel.Suppliers = _mapper.Map<IEnumerable<SupplierViewModel>>(suppliers);
            return productViewModel;
        }

        private async Task<ProductViewModel> PopulateSuppliersAsync(ProductViewModel product)
        {
            var suppliers = await _suppliersRepository.GetAllAsync();

            product.Suppliers = _mapper.Map<IEnumerable<SupplierViewModel>>(suppliers);
            return product;
        }

        private async Task<bool> UploadArchiveAsync(IFormFile archive, string imgPrefix)
        {
            if (archive.Length <= 0)
                return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefix + archive.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await archive.CopyToAsync(stream);
            }

            return true;
        }
    }
}