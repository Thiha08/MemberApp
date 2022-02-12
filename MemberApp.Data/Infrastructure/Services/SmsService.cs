using MemberApp.Data.Infrastructure.Core.Settings;
using MemberApp.Data.Infrastructure.Services.Abstract;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MemberApp.Data.Infrastructure.Services
{
    public class SmsService : ISmsService
    {
        private readonly SmsSettings _smsSettings;
        private readonly IHttpClientFactory _clientFactory;

        public SmsService(IOptions<SmsSettings> optionsAccessor, IHttpClientFactory clientFactory)
        {
            _smsSettings = optionsAccessor.Value;
            _clientFactory = clientFactory;
        }

        public async Task SendSMSAsync(string to, string message)
        {
            var client = _clientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _smsSettings.Key);

            var result = await client.PostAsync($"{_smsSettings.Url}/v2/send",
                new StringContent(JsonConvert.SerializeObject(new { to, message, _smsSettings.Sender }), Encoding.UTF8, "application/json"));
        }
    }
}
