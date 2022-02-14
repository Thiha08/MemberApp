namespace MemberApp.Web.ViewModels.RequestDTOs
{
    public class LoginWithOTPRequest
    {
        public string PhoneNumber { get; set; }
        public string OTPCode { get; set; }
    }
}
