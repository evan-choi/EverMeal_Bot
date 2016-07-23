using System.ComponentModel;

namespace System
{
    public static class CastEx
    {
        public static T TryCast<T>(this object obj, T defaultValue = default(T))
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            
            if (converter != null)
            {
                try
                {
                    return (T)converter.ConvertFrom(obj);
                }
                catch
                { }
            }

            return defaultValue;
        }
    }
}
