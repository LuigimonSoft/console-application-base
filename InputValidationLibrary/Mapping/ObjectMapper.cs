using System;
using System.Reflection;
using InputValidationLibrary.Attributes;

namespace InputValidationLibrary.Mapping
{
    public static class ObjectMapper
    {
        public static T MapFromParameters<T>(string[] parameters) where T : new()
        {
            var obj = new T();
            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
                if (columnAttr != null)
                {
                    var position = columnAttr.Position;

                    if (position < parameters.Length)
                    {
                        var parameterValue = parameters[position];
                        try
                        {
                            var convertedValue = Convert.ChangeType(parameterValue, prop.PropertyType);
                            prop.SetValue(obj, convertedValue);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Error converting value '{parameterValue}' for property '{prop.Name}': {ex.Message}");
                        }
                    }
                }
            }

            return obj;
        }
    }
}

