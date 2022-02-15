using System;

namespace MemberApp.Model.Constants
{
    public static class Constants
    {
        public const string DeletedProperty = "DP";

        public const string SystemTimeZone = "Myanmar Standard Time";

        public static string GenerateOTPCode
        {
            get
            {
                var random = new Random();
                var verificationCode = random.Next(1000, 9999).ToString("D4");
                return verificationCode;
            }
        }

        public const int OTPCodeExpirySeconds = 600;
    }
}
