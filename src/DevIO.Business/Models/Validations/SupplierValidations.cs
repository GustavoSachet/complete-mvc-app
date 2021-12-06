using DevIO.Business.Models.Validations.Documents;
using FluentValidation;

namespace DevIO.Business.Models.Validations
{
    public class SupplierValidations : AbstractValidator<Supplier>
    {
        public SupplierValidations()
        {
            RuleFor(f => f.Name)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 100)
                .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            When(f => f.SupplierType == SupplierType.Person, () =>
            {
                RuleFor(f => f.Document.Length).Equal(CPFValidation.CPFSize)
                    .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");
                RuleFor(f=> CPFValidation.Validate(f.Document)).Equal(true)
                    .WithMessage("O documento fornecido é inválido.");
            });

            When(f => f.SupplierType == SupplierType.Entity, () =>
            {
                RuleFor(f => f.Document.Length).Equal(CnpjValidacao.TamanhoCnpj)
                    .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");
                RuleFor(f => CnpjValidacao.Validate(f.Document)).Equal(true)
                    .WithMessage("O documento fornecido é inválido.");
            });
        }
    }
}