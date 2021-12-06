using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.App.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly INotifier _notificador;

        protected BaseController(INotifier notificador)
        {
            _notificador = notificador;
        }

        protected bool ValidOperation()
        {
            return !_notificador.HasNotifications();
        }
    }
}