using HyperKala.Application.Interfaces;

namespace HyperKala.Application.Services
{
    public class SmsService : ISmsService
    {

        private string ApiKey = "***";

        public async Task SendVerificationCode(string mobile, string activeCode)
        {
            var KavenegarAPI = new Kavenegar.KavenegarApi(ApiKey);

            await KavenegarAPI.VerifyLookup(mobile, activeCode, "NameOfTemp");
        }
    }
}
