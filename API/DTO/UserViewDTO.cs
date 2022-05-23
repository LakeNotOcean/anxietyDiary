using System;
using System.Collections.Generic;

namespace API.DTO
{
    public class UserViewDTO
    {

        public string userName { get; set; }
        public List<DiaryViewDTO> diariesViews { get; set; }
    }

    public class DiaryViewDTO
    {
        public string diaryName { get; set; }
        public DateTime lastViewDate { get; set; }
        public DateTime lastModifyDate { get; set; }
    }
}