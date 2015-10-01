using System.Runtime.InteropServices;

namespace NBlast.Rest.Tools
{
    public static class ObjectExtensions
    {
        public static bool IsNumber(this object value)
        {
            return value.IsByte() 
                || value.IsInt() 
                || value.IsFloat() 
                || value.IsDecimal() 
                || value.IsDouble() 
                || value.IsLong();
        }

        public static bool IsInt(this object value)
        {
            return value is short
                || value is ushort
                || value is int
                || value is uint;
        }

        public static bool IsFloat(this object value)
        {
            return value is float;
        }
        public static bool IsDouble(this object value)
        {
            return value is double;
        }
        public static bool IsDecimal(this object value)
        {
            return value is decimal;
        }
        public static bool IsLong(this object value)
        {
            return value is long || value is ulong;
        }
        public static bool IsByte(this object value)
        {
            return value is sbyte || value is byte;
        }
    }

}