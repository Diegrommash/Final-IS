using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Observable
{
    public interface IOrdenObserver
    {
        void OnOrdenRecibida(string orden);
    }
}
