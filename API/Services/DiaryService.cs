using System.Collections.Generic;
using System.Reflection;
using API.Extensions;
using Persistance;

namespace API.Services
{
    public class DiaryService
    {
        private readonly Dictionary<string, TypeInfo> diariesDictionary;

        public DiaryService(DataContext context)
        {
            diariesDictionary = context.GetDbSetDiaries();
        }

        public TypeInfo getDiaryTypeByName(string name)
        {
            return diariesDictionary[name];
        }
    }
}