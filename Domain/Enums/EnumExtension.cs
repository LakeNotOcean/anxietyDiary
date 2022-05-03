using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Domain.Enums
{
    public static class EnumExtensions
    {
        static EnumExtensions()
        {
            attributes = new Dictionary<Type, Dictionary<string, DescriptionAttribute>>();
        }
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();


            if (!attributes.ContainsKey(type))
            {
                var fields = type.GetFields();
                attributes[type] = new Dictionary<string, DescriptionAttribute>();
                foreach (var f in fields)
                {
                    var attribute = (DescriptionAttribute)f.GetCustomAttribute(typeof(DescriptionAttribute), false);
                    attributes[type][f.Name] = attribute;
                }
            }
            var field = type.GetField(value.ToString());
            return attributes[type][field.Name]?.Description ?? "";

        }
        public static T GetValueFromDescription<T>(string description) where T : Enum
        {
            var type = typeof(T);
            if (!attributes.ContainsKey(type))
            {
                var fields = type.GetFields();
                attributes[type] = new Dictionary<string, DescriptionAttribute>();
                foreach (var f in fields)
                {
                    var attribute = (DescriptionAttribute)f.GetCustomAttribute(typeof(DescriptionAttribute), false);
                    attributes[type][f.Name] = attribute;
                }
            }
            foreach (var dict in attributes[type])
            {
                if (dict.Value.Description == description)
                    return (T)Enum.Parse(type, dict.Key);
            }
            throw new ArgumentException("Not found.", nameof(description));
        }
        private static Dictionary<Type, Dictionary<string, DescriptionAttribute>> attributes;
    }

}