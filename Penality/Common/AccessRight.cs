using System;
using System.Web;
using System.Web.Profile;
using System.Web.Security;

public class AccessRight
{
    public static bool CanAccessResource()
    {
        //var strCurrentUser = HttpContext.Current.Session["CurrentUser"].ToString();
        //if (!Roles.IsUserInRole(strCurrentUser, "Traffic Penality Registration") &&
        //    !Roles.IsUserInRole(strCurrentUser, "Cashier"))
        //    return false;

        return true;
    }

    public static int GetLocation()
    {
        //return 1;
        var strCurrentUser = HttpContext.Current.Session["CurrentUser"].ToString();
        var p = ProfileBase.Create(strCurrentUser);
        //var staff = ((ProfileGroupBase)(p.GetProfileGroup("Staff")));
        //string strFullname = (string)staff.GetPropertyValue("FullName");
        var org = p.GetProfileGroup("Organization");
        var orgCode = (string)org.GetPropertyValue("Code");
        if (!string.IsNullOrEmpty(orgCode))
            return Convert.ToInt32(orgCode);
        return 0;
    }
}
