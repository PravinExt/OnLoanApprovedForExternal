using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace OnLoanApprovedForExternal
{

    public class Function
    {

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(Loan input, ILambdaContext context)
        {
            input.LoanApplication_Status = 8;
            input.LoanApplication_BankerComment = "Closed by External Service";

            string json = JsonConvert.SerializeObject(input);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            string CallBackUrl = "https://g9yh14f7ve.execute-api.ap-south-1.amazonaws.com/Authorizeddev/creditapproval/creditor";
            //string url = input.CreditorCallBackURL + "/" + input.CreditorAssigned_ID.ToString();
            string url = CallBackUrl + "/" + input.CreditorAssigned_ID.ToString();
            var client = new HttpClient();

            Uri myURI = new Uri(url);
            //Need creditor Authorization access token
            client.DefaultRequestHeaders.Add("Authorization", "eyJraWQiOiJXSlpET21BQ0RuS3FHVVhZU2VFXC9pU0J5Y2VRS0xLNlJXdmFiK2pXcDFyWT0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiIyODEwZmM1OS1lZjNiLTRjNjctYmY5Ni0xMzEzZjExYjdiMzUiLCJ0b2tlbl91c2UiOiJhY2Nlc3MiLCJzY29wZSI6ImF3cy5jb2duaXRvLnNpZ25pbi51c2VyLmFkbWluIHBob25lIG9wZW5pZCBwcm9maWxlIGVtYWlsIiwiYXV0aF90aW1lIjoxNjMzNjY3MzMzLCJpc3MiOiJodHRwczpcL1wvY29nbml0by1pZHAuYXAtc291dGgtMS5hbWF6b25hd3MuY29tXC9hcC1zb3V0aC0xX281WHZTRDluOCIsImV4cCI6MTYzMzc1MzczMywiaWF0IjoxNjMzNjY3MzMzLCJ2ZXJzaW9uIjoyLCJqdGkiOiJhYzQ2YThjYS03YmNlLTQyNzEtOTEzYy1kODU0ZjA4NGY2ZTYiLCJjbGllbnRfaWQiOiI0Y2EwNDU1ajM2cXVyNmVtdWY0bzhjZmxrYyIsInVzZXJuYW1lIjoiY3JlZGl0b3IxIn0.vKCKQEy5BDGOP3kuoWUUCIyR6u8q9VazYlt5jVZWzbLigMm4vFiXEBLB4Nxm75zV7BsxVOszuo7jWkJ0_HIiPuBEfw-jdXEYE674cxgIuETWuAsZEWJIG5KlPSgLH6cmpW90k9-Mur4jwMHLgc8jX0osIe8gZPpHnIs-5mmGc8U9UVQB16MqUk7-EXF_r9UUeiYboG05zBOzC7nWUl0Mw61Sbt6Eb11BtmSOiZt2xtt_ZEN66tcXckvuVL8DiezEuyn7c1ebuBgVUFQbVN7okLMcWSw6X9OEalNl4PiuKj3t29rLv6T9K86972Ocq8t4tAasnOlGSzTDo1OJ8x06AA");

            var response = await client.PutAsync(myURI.AbsoluteUri, data);

        }
    }
}
