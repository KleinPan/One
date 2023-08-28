using System;

namespace One.Control.Controls.Dragablz.Referenceless
{
    internal interface ICancelable : IDisposable
    {
        bool IsDisposed { get; }
    }
}