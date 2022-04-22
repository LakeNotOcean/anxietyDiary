namespace API.DTO
{
    public class UserDTO
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }
        public string SecondName { get; set; }

        public RoleDTO Role { get; set; }

        public string Token { get; set; }
    }
}