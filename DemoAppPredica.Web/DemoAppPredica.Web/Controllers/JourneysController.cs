using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
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
    [Authorize(Roles = "Admin, Writer")]
    public class JourneysController : Controller
    {
        [Authorize(Roles = "Admin")]
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
                    await DeserializeReponseJourneyList(response, itemList);

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

                return BadRequest();
            }

            //
            // If the call failed for any other reason, show the user an error.
            //
            return View("Error");
        }

        private static async Task DeserializeReponseJourneyList(HttpResponseMessage response, List<Journey> itemList)
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
                newItem.Id = Int32.Parse(responseElement["id"]);
                newItem.IsValid = Boolean.Parse(responseElement["isValid"]);
                newItem.Cost = Int32.Parse(responseElement["cost"]);
                newItem.UserId = Guid.Parse(responseElement["userId"]);
                itemList.Add(newItem);
            }
        }

        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Updating()
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
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, AzureAdOptions.Settings.DemoPredicaAppApiBaseAddress + $"/api/journeys/user?userId={userObjectID}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    await DeserializeReponseJourneyList(response, itemList);

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

                return BadRequest();
            }

            //
            // If the call failed for any other reason, show the user an error.
            //
            return View("Error");
        }

        [Authorize(Roles = "Admin, Writer")]
        public async Task<IActionResult> Edit(int journeyId)
        {

            AuthenticationResult result = null;
            Journey item = new Journey();

            try
            {
                // Because we signed-in already in the WebApp, the userObjectId is know
                string userObjectID = (User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))
                    ?.Value;

                // Using ADAL.Net, get a bearer token to access the TodoListService
                AuthenticationContext authContext = new AuthenticationContext(AzureAdOptions.Settings.Authority,
                    new NaiveSessionCache(userObjectID, HttpContext.Session));
                ClientCredential credential = new ClientCredential(AzureAdOptions.Settings.ClientId,
                    AzureAdOptions.Settings.ClientSecret);
                result = await authContext.AcquireTokenSilentAsync(AzureAdOptions.Settings.DemoPredicaAppApiResourceId,
                    credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
                    AzureAdOptions.Settings.DemoPredicaAppApiBaseAddress + $"/api/journeys/id?journeyId={journeyId}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var newItem = await DeserializeResponseJourneyObject(response);

                    return View(newItem);
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
                ViewBag.ErrorMessage = "AuthorizationRequired";
                return View(item);
            }

            //
            // If the call failed for any other reason, show the user an error.
            //
            return View("Error");
        }

        private static async Task<Journey> DeserializeResponseJourneyObject(HttpResponseMessage response)
        {
            Dictionary<String, String> responseElements = new Dictionary<String, String>();
            JsonSerializerSettings settings = new JsonSerializerSettings();
            String responseString = await response.Content.ReadAsStringAsync();
            responseElements =
                JsonConvert.DeserializeObject<Dictionary<String, String>>(responseString, settings);

            Journey newItem = new Journey();
            newItem.DestinationCountry = responseElements["destinationCountry"];
            newItem.Name = responseElements["name"];
            newItem.Id = Int32.Parse(responseElements["id"]);
            newItem.IsValid = Boolean.Parse(responseElements["isValid"]);
            newItem.Cost = Int32.Parse(responseElements["cost"]);
            newItem.UserId = Guid.Parse(responseElements["userId"]);
            return newItem;
        }

        [Authorize(Roles = "Admin, Writer")]
        public async Task<IActionResult> Update(Journey journey)
        {
            if (ModelState.IsValid)
            {
                //
                // Retrieve the user's tenantID and access token since they are parameters used to call the To Do service.
                //
                AuthenticationResult result = null;     
                try
                {
                    string userObjectID = (User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;
                    AuthenticationContext authContext = new AuthenticationContext(AzureAdOptions.Settings.Authority, new NaiveSessionCache(userObjectID, HttpContext.Session));
                    ClientCredential credential = new ClientCredential(AzureAdOptions.Settings.ClientId, AzureAdOptions.Settings.ClientSecret);
                    result = await authContext.AcquireTokenSilentAsync(AzureAdOptions.Settings.DemoPredicaAppApiResourceId, credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));

                    // Forms encode todo item, to POST to the todo list web api.
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(journey), System.Text.Encoding.UTF8, "application/json");

                    //
                    // Add the item to user's To Do List.
                    //
                    HttpClient client = new HttpClient();
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, AzureAdOptions.Settings.DemoPredicaAppApiBaseAddress + "/api/journeys");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                    request.Content = content;
                    HttpResponseMessage response = await client.SendAsync(request);

                    //
                    // Return the To Do List in the view.
                    //
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }

                    //
                    // If the call failed with access denied, then drop the current access token from the cache, 
                    //     and show the user an error indicating they might need to sign-in again.
                    //
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
//                        return ProcessUnauthorized(itemList, authContext);
                        return Unauthorized();
                    }
                }
                catch (Exception)
                {
                    //
                    // The user needs to re-authorize.  Show them a message to that effect.
                    //
                    Journey newItem = new Journey();
                    newItem.DestinationCountry = "(No items in list)";
                    ViewBag.ErrorMessage = "AuthorizationRequired";
                    return BadRequest();
                }
                //
                // If the call failed for any other reason, show the user an error.
                //
                return View("Error");
            }
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