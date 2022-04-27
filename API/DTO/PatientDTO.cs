using System;

namespace API.DTO
{
    public class PatientDTO : UserInfoDTO
    {
        public DateTime LastModifyTime { get; set; }
    }
}