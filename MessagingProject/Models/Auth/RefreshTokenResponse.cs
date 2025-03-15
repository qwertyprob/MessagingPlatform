namespace MessagingProject.Models.Auth
{
    public class RefreshTokenResponse
    {
        public required int ErrorCode { get; set; }
        public required string ErrorMessage { get; set; }
        public required string Token { get; set; }
        public bool IsSuccess => ErrorCode == 0 && !string.IsNullOrEmpty(Token);
    }

}
