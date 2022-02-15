using MemberApp.Model.Constants;
using System;
using System.Reflection;

namespace MemberApp.Data.Infrastructure.Core.Extensions
{
    public static class ReflectionExtensions
    {
        public static void TrySetProtectionValue(object obj, string property, string value)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
            {
                try
                {
                    prop.SetValue(obj, string.IsNullOrWhiteSpace(value) ? Constants.DeletedProperty : value, null);
                }
                catch (Exception e) { }
            }
        }
    }
}
