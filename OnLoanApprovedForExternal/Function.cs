using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Runtime;
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
            input.LoanApplication_Status = 8; //Loan_Status 8 i.e. Closed by External Service
            input.LoanApplication_BankerComment = "Closed by External Service"; 

            string json = JsonConvert.SerializeObject(input);
            string StepBodyName = Environment.GetEnvironmentVariable("StepBodyName");
            string StateMachineArn = Environment.GetEnvironmentVariable("StateMachineArn");
            string ExternalAccessKey = Environment.GetEnvironmentVariable("ExternalAccessKey");
            string ExternalSecreteKey = Environment.GetEnvironmentVariable("ExternalSecreteKey");

            var awsCredentials = new BasicAWSCredentials(ExternalAccessKey, ExternalSecreteKey);
            var awsclient = new Amazon.StepFunctions.AmazonStepFunctionsClient(awsCreden‌​tials, Amazon.RegionEndpoint.APSouth1);

            Amazon.StepFunctions.Model.StartExecutionRequest req = new Amazon.StepFunctions.Model.StartExecutionRequest();

            req.Input = json;
            req.Name = StepBodyName;
            req.StateMachineArn = StateMachineArn;

            var aws_response = await awsclient.StartExecutionAsync(req);

        }
    }
}
