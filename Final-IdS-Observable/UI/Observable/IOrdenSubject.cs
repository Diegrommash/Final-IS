using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Observable
{
    public interface IOrdenSubject
    {
        void Suscribir(IOrdenObserver observador);
        void Desuscribir(IOrdenObserver observador);
        void Notificar(string orden);
    }
}
