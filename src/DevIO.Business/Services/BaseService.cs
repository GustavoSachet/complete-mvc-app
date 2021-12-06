using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Notifications;
using FluentValidation;
using FluentValidation.Results;

namespace DevIO.Business.Services
{
    public abstract class BaseService
    {
        private readonly INotifier _notifier;

        private const string DefaultMessage = "Recurso não encontrado!";

        protected BaseService(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected void Notify(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notify(error.ErrorMessage);
            }
        }

        protected void Notify(string message = DefaultMessage)
        {
            _notifier.Handle(new Notification(message));
        }

        protected bool ExecuteValidation<V, E>(V validation, E entity) where V : AbstractValidator<E> where E : Entity
        {
            var validator = validation.Validate(entity);

            if(validator.IsValid)
                return true;

            Notify(validator);

            return false;
        }
    }
}