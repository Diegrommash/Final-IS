using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstracciones
{
    public interface IClonable<T>
    {
        T Clonar();
    }
}
