using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DemoAppPredica.Models.Models.Journeys;
using DemoAppPredica.Web.Extensions;
using DemoAppPredica.Web.Utils;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;

namespace DemoAppPredica.Web.Controllers
{
    [Authorize]
    public class JourneysController : Controller
    {
        public async Task<IActionResult> Index()
        {
            AuthenticationResult result = null;
            List<Journey> itemList = new List<Journey>();

            try
            {
                // Because we signed-in already in the WebApp, the userObjectId is know
                string userObjectID = (User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))
                    ?.Value;

                // Using ADAL.Net, get a bearer token to access the TodoListService
                AuthenticationContext authContext = new AuthenticationContext(AzureAdOptions.Settings.Authority, new NaiveSessionCache(userObjectID, HttpContext.Session));
                ClientCredential credential = new ClientCredential(AzureAdOptions.Settings.ClientId, AzureAdOptions.Settings.ClientSecret);
                result = await authContext.AcquireTokenSilentAsync(AzureAdOptions.Settings.DemoPredicaAppApiResourceId, credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, AzureAdOptions.Settings.DemoPredicaAppApiBaseAddress + "/api/journeys");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    List<Dictionary<String, String>> responseElements = new List<Dictionary<String, String>>();
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    String responseString = await response.Content.ReadAsStringAsync();
                    responseElements = JsonConvert.DeserializeObject<List<Dictionary<String, String>>>(responseString, settings);
                    foreach (Dictionary<String, String> responseElement in responseElements)
                    {
                        Journey newItem = new Journey();
                        newItem.DestinationCountry = responseElement["destinationCountry"];
                        newItem.Name = responseElement["name"];
                        itemList.Add(newItem);
                    }

                    return View(itemList);
                }
            }
            catch (Exception e)
            {
                if (HttpContext.Request.Query["reauth"] == "True")
                {
                    //
                    // Send an OpenID Connect sign-in request to get a new set of tokens.
                    // If the user still has a valid session with Azure AD, they will not be prompted for their credentials.
                    // The OpenID Connect middleware will return to this controller after the sign-in response has been handled.
                    //
                    return new ChallengeResult(OpenIdConnectDefaults.AuthenticationScheme);
                }
                //
                // The user needs to re-authorize.  Show them a message to that effect.
                //
                Journey newItem = new Journey();
                newItem.DestinationCountry = "(Sign-in required to view to do list.)";
                itemList.Add(newItem);
                ViewBag.ErrorMessage = "AuthorizationRequired";
                return View(itemList);
            }

            //
            // If the call failed for any other reason, show the user an error.
            //
            return View("Error");
        }

        private ActionResult ProcessUnauthorized(List<Journey> itemList, AuthenticationContext authContext)
        {
            var todoTokens = authContext.TokenCache.ReadItems().Where(a => a.Resource == AzureAdOptions.Settings.DemoPredicaAppApiResourceId);
            foreach (TokenCacheItem tci in todoTokens)
                authContext.TokenCache.DeleteItem(tci);

            ViewBag.ErrorMessage = "UnexpectedError";
            Journey newItem = new Journey();
            newItem.DestinationCountry = "(No items in list)";
            itemList.Add(newItem);
            return View(itemList);
        }
    }
}