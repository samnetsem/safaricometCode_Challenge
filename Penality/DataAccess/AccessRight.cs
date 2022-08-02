using System;
using System.Web;
using System.Web.Profile;
using System.Web.Security;

namespace MotiSectorAPI.DataAccess
{
    public class AccessRight
    {
        public static bool CanAccessResource()
        {
            string strCurrentUser;
            try
            {
                strCurrentUser = HttpContext.Current.Session["CurrentUser"].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            if (!Roles.IsUserInRole(strCurrentUser, "Traffic MotiSectorAPI Registration") &&
                !Roles.IsUserInRole(strCurrentUser, "Cashier"))
                return false;

            return true;
        }

        public static bool CanAccessPaymentResource()
        {
            string strCurrentUser;
            try
            {
                strCurrentUser = HttpContext.Current.Session["CurrentUser"].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            if (!Roles.IsUserInRole(strCurrentUser, "Cashier"))
                return false;

            return true;
        }


        public static bool CanAccessMotiSectorAPIResource()
        {
            string strCurrentUser;
            try
            {
                strCurrentUser = HttpContext.Current.Session["CurrentUser"].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            if (!Roles.IsUserInRole(strCurrentUser, "Traffic MotiSectorAPI Registration"))
                return false;
            return true;
        }


        public static bool CanAccessTemporaryDriverRegistrationResource()
        {
            string strCurrentUser;
            try
            {
                strCurrentUser = HttpContext.Current.Session["CurrentUser"].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            if (!Roles.IsUserInRole(strCurrentUser, "Driver Data Encoder"))
                return false;

            return true;
        }

        public static bool CanAccessTransactionResourceForVoid()
        {
            string strCurrentUser;
            try
            {
                strCurrentUser = HttpContext.Current.Session["CurrentUser"].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            if (!Roles.IsUserInRole(strCurrentUser, "Cashier"))
                return false;

            return true;
        }

        public static bool CanAccessResourceForReversal()
        {
            string strCurrentUser;
            try
            {
                strCurrentUser = HttpContext.Current.Session["CurrentUser"].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            if (!Roles.IsUserInRole(strCurrentUser, "MotiSectorAPI Reversal"))
                return false;

            return true;
        }




        public static string GetLocationName()
        {
            try
            {
                var strCurrentUser = HttpContext.Current.Session["CurrentUser"].ToString();
                var p = ProfileBase.Create(strCurrentUser);
                //var staff = ((ProfileGroupBase)(p.GetProfileGroup("Staff")));
                //string strFullname = (string)staff.GetPropertyValue("FullName");
                var org = p.GetProfileGroup("Organization");
                var orgName = (string)org.GetPropertyValue("Name");
                if (!string.IsNullOrEmpty(orgName))
                    return orgName;
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }


        public static string GetLocation()
        {
            try
            {
                var strCurrentUser = HttpContext.Current.Session["CurrentUser"].ToString();
                var p = ProfileBase.Create(strCurrentUser);
                //var staff = ((ProfileGroupBase)(p.GetProfileGroup("Staff")));
                //string strFullname = (string)staff.GetPropertyValue("FullName");
                var org = p.GetProfileGroup("MOTBranches");
                var orgCode = (string)org.GetPropertyValue("Code");
                if (!string.IsNullOrEmpty(orgCode))
                    return Convert.ToString(orgCode);
                return "";
            }
            catch
            {
                return "";
            }
        }
    }
}