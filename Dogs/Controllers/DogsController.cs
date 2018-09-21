
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Dogs.Models;
using Dogs.Utilities.WebAPI;

namespace Dogs.Controllers
{
    public class DogsController : Controller
    {
        string szBaseURL = ConfigurationManager.AppSettings["DogsAPIHostPath"];
        WebAPIHelper webAPIHelper;
        public DogsController()
        {            
            webAPIHelper = new WebAPIHelper();
        }

        public async Task<ActionResult> Index()
        {
            //Deserializing the response recieved from web api and storing into the JsonResponseObject 
            JsonResponseListObject lstResponseObject =  await webAPIHelper.JsonRestGet<JsonResponseListObject>(szBaseURL, PublicWebApiUrl.API_LIST_OF_BREEDS);
            // the JsonRestGet could ALSO return an error collection if something went wrong in the API and we could poll that and return an Error view as well.
            if (!(string.IsNullOrEmpty(lstResponseObject.status)) && lstResponseObject.status.ToUpper() == "SUCCESS")
            {
                // Create list of Dog Breeds
                List<Dog> lstDogs = lstResponseObject.message.Select(p => new Dog() { Breed = p.ToString() }).ToList();

                //returning the Breed list to view  
                return View(lstDogs);
            }
            return View("Error");
            
        }
        public async Task<ActionResult> Details(string id)
        {
            //check for null or empty id value
            if (id == null || id.Length == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          
            JsonResponseObject imageResponse = await webAPIHelper.JsonRestGet<JsonResponseObject>(szBaseURL, (PublicWebApiUrl.API_RANDOM_BREED_IMAGE).Replace("{var-id}", id));
            // check response for error 
            // the JsonRestGet could ALSO return an error collection if something went wrong in the API and we could poll that and return an Error view as well.
            if (!(string.IsNullOrEmpty(imageResponse.status)) && imageResponse.status.ToUpper() == "SUCCESS")
            {
                // Instantiate new Dog Object and Assign Image & Breed
                Dog dog = new Dog
                {
                    ImageUrl = imageResponse.message,
                    Breed = id
                };
                //returning the Dog Object To The View  
                return View(dog);
            }
            return View("Error");
        }

        public ActionResult Error()
        {
            var qs = HttpUtility.ParseQueryString(Request.Url.Query);
            var errorPath = qs["aspxerrorpath"];

            return View(model: errorPath);
        }

        public ActionResult NotFound()
        {
            return View();
        }

    }
}