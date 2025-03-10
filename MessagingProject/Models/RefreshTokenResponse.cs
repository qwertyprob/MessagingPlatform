namespace MessagingProject.Models
{
    public class RefreshTokenResponse
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }

        public bool IsSuccess => ErrorCode == 0 && !string.IsNullOrEmpty(Token);
    }

}
