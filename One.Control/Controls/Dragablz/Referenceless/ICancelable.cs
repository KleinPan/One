using System;
using System.Collections.Generic;
using System.Text;

namespace One.Control.Controls.Dragablz.Referenceless
{
    internal interface ICancelable : IDisposable
    {
        bool IsDisposed { get; }
    }
}
