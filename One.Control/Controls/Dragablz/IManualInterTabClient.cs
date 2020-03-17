using System;
using System.Collections.Generic;
using System.Text;

namespace One.Control.Controls.Dragablz
{
    public interface IManualInterTabClient : IInterTabClient
    {
        void Add(object item);
        void Remove(object item);
    }
}
