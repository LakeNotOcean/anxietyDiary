using System;
using System.Collections.Generic;
using System.Reflection;
using API.Extensions;
using Persistance;

namespace API.Services
{
    public class DiaryService
    {
        private readonly Dictionary<string, DiaryProperty> diariesDictionary;

        public DiaryService(Dictionary<string, DiaryProperty> diaries)
        {
            diariesDictionary = diaries;
        }

        public DiaryProperty getDiaryTypeByName(string name)
        {
            return diariesDictionary[name];
        }
    }
}