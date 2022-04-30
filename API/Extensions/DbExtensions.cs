using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Domain.Diaries;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace API.Extensions
{
    public static class DbExtensions
    {
        public static Dictionary<string, TypeInfo> GetDbSetDiariesTypes(this DataContext context)
        {
            var properties = context.GetType().GetProperties();

            var diariesDictionary = new Dictionary<string, TypeInfo>();

            foreach (var propery in properties)
            {
                var setType = propery.PropertyType;
                var isDbSet = setType.IsGenericType && (typeof(DbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition()));
                if (!isDbSet)
                {
                    continue;
                }
                var diaryType = setType.GetGenericArguments()[0].GetTypeInfo();
                var diaryDescription = diaryType.GetCustomAttribute<DescriptionAttribute>();
                if (diaryDescription is not null)
                {
                    diariesDictionary[diaryDescription.Description] = diaryType;
                }

            }
            return diariesDictionary;
        }
    }
}