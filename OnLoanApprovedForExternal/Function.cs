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

            string CallBackUrl = Environment.GetEnvironmentVariable("CreditApprovalURL");
            //string CallBackUrl = "https://g9yh14f7ve.execute-api.ap-south-1.amazonaws.com/Authorizeddev/creditapproval/creditor";
            string url = CallBackUrl + "/" + input.CreditorAssigned_ID.ToString();
            var client = new HttpClient();

            Uri myURI = new Uri(url);
            //Need creditor Authorization access token
            client.DefaultRequestHeaders.Add("Authorization", "eyJraWQiOiJXSlpET21BQ0RuS3FHVVhZU2VFXC9pU0J5Y2VRS0xLNlJXdmFiK2pXcDFyWT0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiIyODEwZmM1OS1lZjNiLTRjNjctYmY5Ni0xMzEzZjExYjdiMzUiLCJldmVudF9pZCI6Ijk0Yzk0MzViLTM2ZmItNDhmOS05MWYwLTY0ODRhNzQ5NzA0ZiIsInRva2VuX3VzZSI6ImFjY2VzcyIsInNjb3BlIjoiYXdzLmNvZ25pdG8uc2lnbmluLnVzZXIuYWRtaW4gcGhvbmUgb3BlbmlkIHByb2ZpbGUgZW1haWwiLCJhdXRoX3RpbWUiOjE2MzQwOTc2NTcsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC5hcC1zb3V0aC0xLmFtYXpvbmF3cy5jb21cL2FwLXNvdXRoLTFfbzVYdlNEOW44IiwiZXhwIjoxNjM0MTg0MDU3LCJpYXQiOjE2MzQwOTc2NTcsInZlcnNpb24iOjIsImp0aSI6ImJmY2Q0NDE2LTcyN2QtNDM3ZC05ZDhiLTdmYWI3OTg4OGRlNCIsImNsaWVudF9pZCI6IjRjYTA0NTVqMzZxdXI2ZW11ZjRvOGNmbGtjIiwidXNlcm5hbWUiOiJjcmVkaXRvcjEifQ.XCc6fioShxcOoeuHHYmaH0efyIp9MqERmaA8QxfkGWdNDLaSgIvQNttcVmEZZspegxFHWbp1jJRI6zrztiYnw4O0uIAL_6lnoBRzouIZth164QAoFmJV3HogerCj8Ot0_P904UbuPEKSNjDvAwaTlvJ2ZoiNGC5-WOioTI7rwCn-keS5imY8imKaXzecVBu6zKpkkgsvzwGSzjzCe4mplVMJvuWZrB_bzNPb18_DcPtHCenqUn3koRoH7tgT3LZkgZt6-Hne4nos2wwpmxQRS429kSJd_hPCWWTtmw4NOMpqL6F5H53Hc2sMSl0z-VDO11YpQ0j2r83pGR3hMPFsDg");

            var response = await client.PutAsync(myURI.AbsoluteUri, data);

        }
    }
}
