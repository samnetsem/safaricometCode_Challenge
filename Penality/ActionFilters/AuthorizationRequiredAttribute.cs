using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MotiSectorAPI.Service;

namespace MotiSectorAPI.ActionFilters
{
    public class AuthorizationRequiredAttribute : ActionFilterAttribute
    {
        private const string Token = "Token";

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            //var provider = new TokenService();

            if (filterContext.Request.Headers.Contains(Token))
            {
                var tokenValue = filterContext.Request.Headers.GetValues(Token).First();
               // var strValidUserName = TokenService.ValidateToken(tokenValue);

                // Validate Token
                //if (string.IsNullOrEmpty(strValidUserName))
                //{
                //    var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                //    {
                //        ReasonPhrase = "Invalid Request"
                //    };
                //    filterContext.Response = responseMessage;
                //}
                //else
                //{
                //    HttpContext.Current.Session["CurrentUser"] = strValidUserName;



                    //ProfileCommon p = new ProfileCommon();
                    //ProfileCommon profile = p.GetProfile(strValidUserName);
                    //if (profile == null)
                    //{
                    //    profile =(ProfileCommon) ProfileBase.Create(strValidUserName);
                    //    var org = ((ProfileGroupBase)(p.GetProfileGroup("Organization")));

                    //}

                    //p = ProfileBase.Create(strValidUserName);

                    //var staff = ((ProfileGroupBase)(p.GetProfileGroup("Staff")));
                    //var org = ((ProfileGroupBase)(p.GetProfileGroup("Organization")));
                    //profile = this.Profile.GetProfile(this.txtUserName.Text);
                    //profile.Organization.Code = hfOrg.Get("Code").ToString();
                    //profile.Save();
               // }
            }
            else
            {
               // filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}