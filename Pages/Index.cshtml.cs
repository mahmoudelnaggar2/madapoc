using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MADAPOC.Pages
{
    public class IndexModel : PageModel
    {
        public string SessionId { get; set; }
        public string OrderId { get; set; }

        public async Task OnGet()
        {       
            string responseContent = "[]";
            var retunURL = "http://madapoc.com/";
            var username = "merchant.5432154321";
            var password = "fe7cb3b543f83442957a4cea7a385d02";        

        Uri baseURL = new Uri("https://test-bsf.mtf.gateway.mastercard.com/api/rest/version/57/merchant/5432154321/session");        

            HttpClient client = new HttpClient();
            OrderId = $"sfda-{DateTime.Now.Ticks}";

            JObject jObject = JObject.FromObject(new
            {
                apiOperation = "CREATE_CHECKOUT_SESSION",
                interaction = new
                {
                    operation = "PURCHASE",
                    returnUrl = retunURL
                },
                order = new
                {
                    amount = "1.00",
                    currency = "SAR",
                    id = $"sfda-{DateTime.Now}",
                    reference = OrderId
                },
                transaction = new { reference = $"sfda-{DateTime.Now}" }
            });

            var stringContent = new StringContent(jObject.ToString(), Encoding.UTF8, "application/json");

            var byteArray = Encoding.ASCII.GetBytes("" + username + ":" + password + "");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            HttpResponseMessage response = await client.PostAsync(baseURL.ToString(), stringContent);

            if (response.IsSuccessStatusCode)
            {
                responseContent = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(responseContent);

                SessionId = json.session.id;
            }
        }
    }
}
