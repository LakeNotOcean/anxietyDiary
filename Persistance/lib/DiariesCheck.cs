using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.DiaryExpensions;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Lib
{
    public static class DiariesCheck
    {
        public static async Task Check(TypeInfo diaryClass, DataContext data)
        {
            var diaryName = diaryClass.GetCustomAttribute<DescriptionAttribute>().Description;
            var description = await data.Descriptions.Include(d => d.ArbitraryColumns)
                .Include(d => d.NonArbitraryColumns)
                .SingleOrDefaultAsync(d => d.ShortName == diaryName);
            if (description is null)
            {
                CheckException(diaryClass, "description is null");
            }
            var columns = description.NonArbitraryColumns.Cast<DiaryColumn>().ToList();
            var properties = diaryClass.GetProperties();

            var dict = new Dictionary<string, PropertyInfo> { };

            foreach (var propery in properties)
            {
                var properyName = propery.GetCustomAttribute<JsonPropertyNameAttribute>();
                if (properyName is not null)
                {
                    if (dict.ContainsKey(properyName.Name))
                    {
                        CheckException(diaryClass, $"multiple key {properyName.Name}");
                    }
                    dict[properyName.Name] = propery;
                }
            }
            checkColumns(diaryClass, columns, dict, TypesOfValues);
            var ArbColumns = description.ArbitraryColumns.Cast<DiaryColumn>().ToList();
            checkColumns(diaryClass, ArbColumns, dict, TypesOfListValues);
        }

        private static void CheckException(TypeInfo diaryClass, string message)
        {
            throw new Exception(message + $" in {diaryClass.Name}");
        }

        private static void checkColumns(TypeInfo diaryClass, List<DiaryColumn> columns, Dictionary<string, PropertyInfo> propertyDict, Dictionary<ColumnValueType, Type> TypesDict)
        {
            foreach (var column in columns)
            {
                if (!propertyDict.ContainsKey(column.ShortName))
                {
                    CheckException(diaryClass, $"not contains {column.ShortName}");
                }
                if (propertyDict[column.ShortName].PropertyType
                    != TypesDict[column.ValueType])
                {
                    CheckException(diaryClass, $"invalid type {column.ShortName}");
                }
            }
        }
        private static readonly Dictionary<ColumnValueType, Type> TypesOfValues = new Dictionary<ColumnValueType, Type>{
        {
                ColumnValueType.String,
                typeof(string)
        },
        {
            ColumnValueType.Int,
            typeof(int)
        },
        {
            ColumnValueType.Bool,
            typeof(bool)
        },
        {
            ColumnValueType.Json,
            typeof(System.Text.Json.Nodes.JsonObject)
        },
        {
            ColumnValueType.Date,
            typeof(System.DateTime)
        }};

        private static readonly Dictionary<ColumnValueType, Type> TypesOfListValues = new Dictionary<ColumnValueType, Type>{
        {
                ColumnValueType.String,
                typeof(List<string>)
        },
        {
            ColumnValueType.Int,
            typeof(List<int>)
        },
        {
            ColumnValueType.Bool,
            typeof(List<bool>)
        }};
    }
}