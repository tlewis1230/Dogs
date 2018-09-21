using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Dogs.Utilities.WebAPI
{
    public class WebAPIHelper
    {
        public WebAPIHelper()
        {
            
        }
        #region JsonRestGet
        public async Task<T> JsonRestGet<T>(string szBaseURL, string szURL)
        {
            HttpResponseMessage responseMessage;
        
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(szBaseURL);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource using HttpClient  
                responseMessage = await client.GetAsync(szURL);
            }

            //Checking if the response is successful or not
            if (responseMessage.IsSuccessStatusCode)
            {
                //Deserializing the response recieved from web api and storing into the Response Object  
                var response = responseMessage.Content.ReadAsStringAsync().Result;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Deserialize<T>(response);
            }
            // if there is an error let's just return T with default values
            // we could also in this case grab the reponse error code & description and write it to a JSON error collection and return it in that form to the calling method
            else
            {                
                return default(T);
            }
            
        }
        #endregion

    }
}