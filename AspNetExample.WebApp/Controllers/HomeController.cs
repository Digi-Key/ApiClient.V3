using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using ApiClient.Models;
using ApiClient.OAuth2;
using ApiClient.OAuth2.Models;
using AspNetExample.WebApp.Models;
using AspNetExample.WebApp.ViewModels;

namespace AspNetExample.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public static ApiClientSettings ClientSettings { get; private set; }

        private static OAuth2Service _oAuth2Service;

        public HomeController()
        {
            // Load OAuth2 settings from config file apiclient.config
            ClientSettings = ApiClientSettings.CreateFromConfigFile();
        }

        public ActionResult Index()
        {
            // Display some of the OAuth2 settings
            return View(SettingsViewModel.Create(ClientSettings));
        }

        [HttpPost]
        public ActionResult Index(SettingsViewModel settingsViewModel)
        {
            ClientSettings.ClientId = settingsViewModel.ClientId;
            ClientSettings.ClientSecret = settingsViewModel.ClientSecret;
            ClientSettings.RedirectUri = settingsViewModel.RedirectUri;
            ClientSettings.Save();  

            UpdateOAuth2Service(ClientSettings);

            return RedirectToAction("GenerateToken");
        }


        public RedirectResult GenerateToken()
        {
            var scopes = "";
            var authUrl = _oAuth2Service.GenerateAuthUrl(scopes);

            return new RedirectResult(authUrl);
        }


        public async Task<ActionResult> FinishAuth()
        {
            UpdateOAuth2Service(ClientSettings);
            string code = Request.QueryString["code"];
            var result = await _oAuth2Service.FinishAuthorization(code);

            ClientSettings.UpdateAndSave(result);

            return RedirectToAction("DisplayInformation", new RouteValueDictionary(result));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult DisplayInformation(OAuth2AccessToken oAuth2AccessToken)
        {
            return View(DisplayInformationViewModel.Create(oAuth2AccessToken,
                                                           ClientSettings.ClientId,
                                                           ClientSettings.ClientSecret));
        }

        public static void UpdateOAuth2Service(ApiClientSettings clientSettings)
        {
            _oAuth2Service = new OAuth2Service(clientSettings);
        }
    }
}
