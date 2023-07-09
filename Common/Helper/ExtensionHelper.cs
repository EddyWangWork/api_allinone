using System;
using System.Collections.Generic;
using System.Text;

namespace demoAPI.Common.Helper
{
    public static class ExtensionHelper
    {
        public static bool IsNullOrEmpty(this object data)
        {
            if (null == data) return true;
            if (data is string && string.IsNullOrEmpty(data.ToString())) return true;
            if (data is DBNull) return true;
            return false;
        }

        public static bool IsNotNullOrEmpty(this object data)
        {
            return !data.IsNullOrEmpty();
        }
    }
}
