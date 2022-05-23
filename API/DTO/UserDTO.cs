using System;

namespace API.DTO
{
    public class UserDTO
    {
        public string userName { get; set; }

        public string firstName { get; set; }
        public string secondName { get; set; }

        public string role { get; set; }

        public string token { get; set; }

        public bool isSearching { get; set; }
        public string description { get; set; }
    }
}