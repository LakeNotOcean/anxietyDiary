using System;
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
        public static Dictionary<string, DiaryProperty> GetDbSetDiariesTypes(this DataContext context)
        {
            var properties = context.GetType().GetProperties();

            var diariesDictionary = new Dictionary<string, DiaryProperty>();

            foreach (var property in properties)
            {
                var setType = property.PropertyType;
                var isDbSet = setType.IsGenericType && (typeof(DbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition()));
                if (!isDbSet)
                {
                    continue;
                }
                var diaryType = setType.GetGenericArguments()[0].GetTypeInfo();
                var diaryDescription = diaryType.GetCustomAttribute<DescriptionAttribute>();
                if (diaryDescription is not null)
                {
                    diariesDictionary[diaryDescription.Description] = new DiaryProperty
                    {
                        PropertyName = property.Name,
                        PropertyTypeInfo = diaryType
                    };
                }

            }
            return diariesDictionary;
        }

    }
    public class DiaryProperty
    {
        public string PropertyName { get; set; }
        public TypeInfo PropertyTypeInfo { get; set; }
    }
}