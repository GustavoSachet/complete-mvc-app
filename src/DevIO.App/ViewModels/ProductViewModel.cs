using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevIO.App.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Fornecedor")]
        public Guid SupplierId { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Name { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Description { get; set; }

        public string Image { get; set; }

        [DisplayName("Imagem")]
        public IFormFile ImageUpload { get; set; }

        [DisplayName("Preço")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal Price { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreationDate { get; set; }

        [DisplayName("Ativo?")]
        public bool Active { get; set; }

        public SupplierViewModel Supplier { get; set; }

        public IEnumerable<SupplierViewModel> Suppliers { get; set; }
    }
}