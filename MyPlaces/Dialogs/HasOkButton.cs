using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPlaces.Dialogs
{
    public interface HasOkButton<T> where T : EventArgs
    {
        event EventHandler<T> OKClick;
    }
}
