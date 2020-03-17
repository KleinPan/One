using System;
using System.Collections.Generic;
using System.Text;

namespace One.Control.Controls.Dragablz.Dockablz
{
    public class LocationReportException : Exception
    {
        public LocationReportException()
        {
        }

        public LocationReportException(string message) : base(message)
        {
        }

        public LocationReportException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
