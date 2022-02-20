namespace MemberApp.Model.ResultModels
{
    public class LoginResult
    {
        public string AccessToken { get; set; }

        public bool IsNotConfirmedByAdmin { get; set; } = true;

        public bool IsLocked { get; set; } = true;

        public bool IsNotValidOTP { get; set; } = true;
    }
}
