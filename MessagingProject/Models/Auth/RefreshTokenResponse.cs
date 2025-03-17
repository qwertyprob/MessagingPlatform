namespace MessagingProject.Models.Auth
{
    public class RefreshTokenResponse : BaseResponseModel
    {
        
        public required string Token { get; set; }
        public bool IsSuccess => ErrorCode == 0 && !string.IsNullOrEmpty(Token);
    }

}
