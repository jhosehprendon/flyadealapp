using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Newskies.WebApi.Extensions
{
    public static class HttpContextExtensions
    {
        public static async Task<string> GetServerIP(this HttpContext context)
        {
            IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync(Dns.GetHostName());
            if (ipHostInfo.AddressList != null && ipHostInfo.AddressList.Length > 0)
            {
                var addressIpv4 = ipHostInfo.AddressList.ToList().Find(p => p.AddressFamily == AddressFamily.InterNetwork);
                var ipStr = addressIpv4 != null ? addressIpv4.ToString() : ipHostInfo.AddressList[0].ToString();
                return ipStr;
            }
            return string.Empty;
        }
    }
}
