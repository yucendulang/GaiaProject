using System;
using System.Collections.Generic;
using System.Text;

namespace GaiaCore.Gaia.CommonException
{
    public class ResourceNotEnoughException : Exception
    {
        public ResourceNotEnoughException(string message) : base(message)
        {
        }
    }
}
