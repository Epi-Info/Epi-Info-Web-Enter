using System;
using System.Text;
using System.Collections;
using System.DirectoryServices;
using System.Security.Principal;
using System.Web.Hosting;
using System.DirectoryServices.AccountManagement;
 

namespace Epi.Web.MVC.Utility
{
    public static class WindowsAuthentication
    {


        public static bool IsAuthenticated(String domain, String username, String pwd, string _path, string _filterAttribute)
        {
            String domainAndUsername = domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);

            try
            {	//Bind to the native AdsObject to force authentication.			
                Object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (null == result)
                {
                    return false;
                }

                //Update the new path to the user in the directory.
                _path = result.Path;
                _filterAttribute = (String)result.Properties["cn"][0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user. " + ex.Message);
            }

            return true;
        }
        public static UserPrincipal GetCurrentUserFromAd(string UserName)
        {
            using (HostingEnvironment.Impersonate())
            {
                var context = new PrincipalContext(ContextType.Domain, UserName.Split('\\')[0].ToString());
                var userPrincipal = new UserPrincipal(context) { SamAccountName = UserName.Split('\\')[1].ToString() };
                var searcher = new PrincipalSearcher { QueryFilter = userPrincipal };
                var results = (UserPrincipal)searcher.FindOne();

                if (results == null)
                {
                    return null;
                }

                return results;
            }
        }
        public static UserPrincipal GetUserFromAd(string UserEmail ,string Domain)
        {
            using (HostingEnvironment.Impersonate())
            {
                try
                {
                    var context = new PrincipalContext(ContextType.Domain, Domain);
                    var userPrincipal = new UserPrincipal(context) { EmailAddress = UserEmail };
                    var searcher = new PrincipalSearcher { QueryFilter = userPrincipal };
                    var results = (UserPrincipal)searcher.FindOne();

                    if (results == null)
                    {
                        return null;
                    }

                    return results;
                }catch (Exception){
                
                return null;
                }
            }
        }
        public static String GetGroups(string _path, string _filterAttribute)
        {
            DirectorySearcher search = new DirectorySearcher(_path);
            search.Filter = "(cn=" + _filterAttribute + ")";
            search.PropertiesToLoad.Add("memberOf");
            StringBuilder groupNames = new StringBuilder();

            try
            {
                SearchResult result = search.FindOne();

                int propertyCount = result.Properties["memberOf"].Count;

                String dn;
                int equalsIndex, commaIndex;

                for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                {
                    dn = (String)result.Properties["memberOf"][propertyCounter];

                    equalsIndex = dn.IndexOf("=", 1);
                    commaIndex = dn.IndexOf(",", 1);
                    if (-1 == equalsIndex)
                    {
                        return null;
                    }

                    groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                    groupNames.Append("|");

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error obtaining group names. " + ex.Message);
            }
            return groupNames.ToString();
        }
    }
        
}