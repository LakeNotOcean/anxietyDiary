using Domain.Enums;

namespace API.DTO
{
    public class RequestDTO
    {
        public UserInfoDTO sourceUser { get; set; }
        public UserInfoDTO? targetUser { get; set; }
        public int requestId { get; set; }
        public RequestsEnum requestType { get; set; }
    }

    public class SendRequestDTO
    {
        public string userName { get; set; }
        public int requestType { get; set; }
    }
}