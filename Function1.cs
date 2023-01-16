using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using ProjectCampaigns.Model;
using ProjectCampaigns.Entities;
using System.Data.SqlClient;
using System.Text.Json;

namespace ClientServerAuth0SQLDbProject.MiniServer
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "delete", "put", Route = "Function1/{action}/{productsId?}")] HttpRequest req, string action, string productsId,
            ILogger log)
        {
            switch (action)
            {
                // Execute the "NewMethod2" method and return its result
                case "creatingCampaign":
                    return await NewMethod2(req);

                // Read the request body, deserialize it into a "Twitter" object and insert the user's support information into the database using the "InsertUserSupportToDb" method
                case "Support":
                    string requestBody1 = await new StreamReader(req.Body).ReadToEndAsync();
                    Twitter SupportCampaignDetails = System.Text.Json.JsonSerializer.Deserialize<Twitter>(requestBody1);
                    if (SupportCampaignDetails.associationName != null && SupportCampaignDetails.email != null && SupportCampaignDetails.userName != null && SupportCampaignDetails.hashtag != null && SupportCampaignDetails.CampaignName != null && SupportCampaignDetails.twitterUsername != null)
                    {
                        MainManager.Instance.Twitter.InsertUserSupportToDb(SupportCampaignDetails.associationName, SupportCampaignDetails.email, SupportCampaignDetails.userName, SupportCampaignDetails.hashtag, SupportCampaignDetails.CampaignName, SupportCampaignDetails.twitterUsername);
                        return new OkObjectResult("This POST request executed successfully");
                    }
                    return new BadRequestObjectResult("Failed POST Request");

                // Execute the "donationMethod" method and return its result
                case "donation":
                    log.LogInformation("C# HTTP trigger function processed a request.");
                    return await donationMethod(req);

                // Execute the "NewMethod1" method and return its result
                case "CompanyOwnerUser":
                    return await NewMethod1(req, log);

                // Execute the "NewMethod" method and return its result
                case "Campaigns":
                    return NewMethod();
                // Read the request body, deserialize it into a "user" object and insert the user's information into the database using the "CreateUsers" method
                case "SingUp":
                    string singUp = await new StreamReader(req.Body).ReadToEndAsync();
                    user newUser = System.Text.Json.JsonSerializer.Deserialize<user>(singUp);
                 

                    if (newUser.userName != null && newUser.cellphoneNumber != null && newUser.email != null && newUser.UserType != null && newUser.twitterUsername != null && newUser.user_id != null)
                    {
                        MainManager.Instance.userNew.CreateUsers(newUser.userName, newUser.cellphoneNumber, newUser.email, newUser.UserType, newUser.twitterUsername, newUser.user_id);


                        return new OkObjectResult("This POST request executed successfully");
                    }
                   


                    break;
                case "Products"://תרומת מוצר
                 if (productsId == null)
                    {
                       
                        log.LogInformation("C# HTTP trigger function processed a request.");

                        return NewMethod3();
                    }

                    else
                    {
                        return NewMethod4(productsId);
                    }
                    

                    break;
                    /*
                case "SalesReports"://תרומת מוצר

                    log.LogInformation("C# HTTP trigger function processed a request.");

                    Dictionary<int, sale> SalesReports = MainManager.Instance.sales.salesDetailsfromSQL();
                    string getcompanyOwnerUser = System.Text.Json.JsonSerializer.Serialize(businessOwner);

                    return new OkObjectResult(getcompanyOwnerUser);
                    break;

                    */
                case "companyOwnerUser"://תרומת מוצר

                    log.LogInformation("C# HTTP trigger function processed a request.");

                     Dictionary<int, businessOwner> businessOwner = MainManager.Instance.businessOwner.businessOwnerDetailsfromSQL();
                     string getcompanyOwnerUser = System.Text.Json.JsonSerializer.Serialize(businessOwner);

                        return new OkObjectResult(getcompanyOwnerUser);
                  break;

               
                case "Sales":
                   

                    Dictionary<int, sale> sale = MainManager.Instance.sales.salesDetailsfromSQL();
                    string getCampaing = System.Text.Json.JsonSerializer.Serialize(sale);

                    return new OkObjectResult(getCampaing);


                    break;


                case "CampaingsTable":


                    Dictionary<int, Campaign> campaings = MainManager.Instance.newcampaing.campaingsTableDetailsfromSQL();
                    string getCampaingTable = System.Text.Json.JsonSerializer.Serialize(campaings);

                    return new OkObjectResult(getCampaingTable);


                    break;

             case "TwitterTable":
                   List<Twitter> Twitter = MainManager.Instance.Twitter.TwitterTableDetailsfromSQL();
                   string getTwitterTable = System.Text.Json.JsonSerializer.Serialize(Twitter);
                   return new OkObjectResult(getTwitterTable);
                     break;




                case "addSale"://תרומת מוצר
                    string requestBody3 = await new StreamReader(req.Body).ReadToEndAsync();
 
    
                    sale newsale = System.Text.Json.JsonSerializer.Deserialize<sale>(requestBody3);
                    if (newsale.buyerName != null && newsale.cellphoneNumber != null && newsale.Email != null && newsale.buyerAddress != null && newsale.CompanyName != null)
                    {
                        MainManager.Instance.newsales.InsertnewSaleseToDb(newsale.buyerName, newsale.cellphoneNumber, newsale.Email, newsale.buyerAddress, newsale.CompanyName);


                        return new OkObjectResult("This POST request executed successfully");
                    }

                    return new BadRequestObjectResult("Failed POST Request");
                    
                    break;

                case "User":


                    Dictionary<int, user> user = MainManager.Instance.userNew.UserDetailsfromSQL();
                    string geUser = System.Text.Json.JsonSerializer.Serialize(user);

                    return new OkObjectResult(geUser);


                    break;

            }
                    return null;


            }

        private static IActionResult NewMethod4(string productsId)
        {
            ////Serialize the products data to a JSON string
            string responseMessage = System.Text.Json.JsonSerializer.Serialize(MainManager.Instance.newDonorDetail.GetProductFromDbById(int.Parse(productsId)));
            //Return an OK result with the serialized data
            return new OkObjectResult(responseMessage);
        }

        private static IActionResult NewMethod3()
        {
            Dictionary<int, Donation> a = MainManager.Instance.newDonorDetail.ProductsDetailsfromSQL();
            string getCampaing = System.Text.Json.JsonSerializer.Serialize(a);

            return new OkObjectResult(getCampaing);
        }

        private static async Task<IActionResult> NewMethod2(HttpRequest req)
        {
            string requestBody1 = await new StreamReader(req.Body).ReadToEndAsync();
            Campaign newCampaignDetails = System.Text.Json.JsonSerializer.Deserialize<Campaign>(requestBody1);
            if (newCampaignDetails.associationName != null && newCampaignDetails.email != null && newCampaignDetails.uri != null && newCampaignDetails.hashtag != null&& newCampaignDetails.CampaignName != null)
            {
                MainManager.Instance.newcampaing.InsertUserMessageToDb(newCampaignDetails.associationName, newCampaignDetails.email, newCampaignDetails.uri, newCampaignDetails.hashtag, newCampaignDetails.CampaignName);


                return new OkObjectResult("This POST request executed successfully");
            }

            return new BadRequestObjectResult("Failed POST Request");
        }

        private static async Task<IActionResult> NewMethod1(HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody2 = await new StreamReader(req.Body).ReadToEndAsync();
            JsonConvert.DeserializeObject(requestBody2);

            List<companyOwnerUser> CompanyOwnerUser = MainManager.Instance.companyOwnerUsersInit();
            string responseMessage = System.Text.Json.JsonSerializer.Serialize(CompanyOwnerUser);

            return new OkObjectResult(responseMessage);
        }

        private static IActionResult NewMethod()
        {

            Dictionary<int,Campaign > a = MainManager.Instance.newcampaing.CampaignDetailsfromSQL();
            string getCampaing = System.Text.Json.JsonSerializer.Serialize(a);

            return new OkObjectResult(getCampaing);

         
        }

        private static async Task<IActionResult> donationMethod(HttpRequest req)
        {
            string donorDetails = await new StreamReader(req.Body).ReadToEndAsync();
            Donation newDonation = System.Text.Json.JsonSerializer.Deserialize<Donation>(donorDetails);
            if (newDonation.CompanyName != null && newDonation.Product != null && newDonation.Email != null && newDonation.Price != null && newDonation.CampaignName != null)
            {
                MainManager.Instance.newDonorDetail.CreateDonation(newDonation.CompanyName, newDonation.CampaignName, newDonation.Product, newDonation.Email, newDonation.Price);


                return new OkObjectResult("This POST request executed successfully");
            }
            else return new OkObjectResult("The operation failed");
        }
    }
    }


