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
        private readonly List<string> diariesNames;

        public DiaryService(Dictionary<string, DiaryProperty> diaries)
        {
            diariesDictionary = diaries;
            diariesNames = new List<string>();

            foreach (var diary in diaries)
            {
                diariesNames.Add(diary.Key);
            }
        }

        public DiaryProperty getDiaryTypeByName(string name)
        {
            return diariesDictionary[name];
        }
        public List<string> getDiariesNames()
        {
            return diariesNames;
        }
    }
}