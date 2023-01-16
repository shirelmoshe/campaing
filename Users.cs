using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjectCampaigns.Entities;
using static System.Collections.Specialized.BitVector32;

namespace PROJECT
{
    public static class Users
    {
        [FunctionName("Users")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "delete", "put", Route = "Users/{action}/{user_id?}")] HttpRequest req, string action, string user_id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            switch (action)
            {
                case "Tweets":
                    // Create a new Twitters object and call the "amountOftweets" method on it
                    Twitters tweetCount = new Twitters();
                    tweetCount.amountOftweets();
                    // Return an "OkObjectResult" with the message "Tweet count updated successfully"
                    return new OkObjectResult("Tweet count updated successfully");
                    break;
                case "AssociationRepresentative":
                    if (user_id != null)
                    {
                        string responseMessage = System.Text.Json.JsonSerializer.Serialize(MainManager.Instance.userNew.GetUserSocialActivistFromDbById(user_id));
                        // Return an "OkObjectResult" with the serialized data
                        return new OkObjectResult(responseMessage);
                    }
                    break;
                case "SocialActivists":
                    // Retrieve the user information from the database using the "GetUserSocialActivistFromDbById" method and serialize the data to a JSON string
                    if (user_id != null)
                    {
                        string responseMessage = System.Text.Json.JsonSerializer.Serialize(MainManager.Instance.userNew.GetUserSocialActivistFromDbById(user_id));
                        // Return an "OkObjectResult" with the serialized data
                        return new OkObjectResult(responseMessage);
                    }


                    break;
                case "CompanyOwnerUser":
                    // Retrieve the user information from the database using the "GetUserSocialActivistFromDbById" method and serialize the data to a JSON string
                    if (user_id != null)
                    {
                        string responseMessage = System.Text.Json.JsonSerializer.Serialize(MainManager.Instance.userNew.GetUserSocialActivistFromDbById(user_id));
                        // Return an "OkObjectResult" with the serialized data
                        return new OkObjectResult(responseMessage);
                    }
                    break;
                case "Admin":
                    // Retrieve the user information from the database using the "GetUserSocialActivistFromDbById" method and serialize the data to a JSON string
                    if (user_id != null)
                    {
                        string responseMessage = System.Text.Json.JsonSerializer.Serialize(MainManager.Instance.userNew.GetUserSocialActivistFromDbById(user_id));
                        // Return an "OkObjectResult" with the serialized data
                        return new OkObjectResult(responseMessage);
                    }
                    break;
                case "MoneyTracking":
                    // Retrieve the user information from the database using the "GetUserSocialActivistFromDbById" method and serialize the data to a JSON string
                    if (user_id != null)
                    {
                        string responseMessage = System.Text.Json.JsonSerializer.Serialize(MainManager.Instance.moneyTracking.GetmoneyTrackingsFromDbById(user_id));
                        // Return an "OkObjectResult" with the serialized data
                        return new OkObjectResult(responseMessage);
                    }
                    break;



                default:
                    // Return a "BadRequestObjectResult" with the message "Invalid action provided, please provide a valid action in the route"
                    return new BadRequestObjectResult("Invalid action provided, please provide a valid action in the route");
            }
            // Return a "NotFoundObjectResult" with the message "User not found with provided id"
            return new NotFoundObjectResult("User not found with provided id");
        }
    }
}
