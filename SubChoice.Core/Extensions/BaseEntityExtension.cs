using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using SubChoice.Core.Data.Entities;

namespace SubChoice.Core.Extensions
{
    public static class BaseEntityExtension
    {
        public static void MapChanges(this IBaseEntity entity, object data)
        {
            foreach (var propertyInfo in data.GetType().GetProperties(BindingFlags.FlattenHierarchy |
                                                                      BindingFlags.Instance |
                                                                      BindingFlags.Public))
            {
                var val = propertyInfo.GetValue(data);
                if (val != null)
                {
                    var correspondingProperty = entity.GetType().GetProperty(propertyInfo.Name);
                    try
                    {
                        correspondingProperty?.SetValue(entity, val, null);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }
    }
}
