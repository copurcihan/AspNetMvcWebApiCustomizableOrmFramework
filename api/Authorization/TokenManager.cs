
using ccoftBLL.USER;
using ccoftOBJ;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;

namespace api.Authorization
{
    public class TokenManager : IHttpModule
    {
        public void Dispose()
        {
        }
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += Context_AuthenticateRequest;
            context.EndRequest += Context_EndRequest;
        }
        private void Context_EndRequest(object sender, EventArgs e)
        {
            var response = HttpContext.Current.Response;
            if (response.StatusCode == 401)
                response.Headers.Add("WWWW-Authenticate", "Basic realm=\"insert for realm \"");
        }
        private void Context_AuthenticateRequest(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;
            var header = request.Headers["Authorization"];

            if (header != null)
            {
                var ParsedValue = AuthenticationHeaderValue.Parse(header);

                if ((ParsedValue.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) && ParsedValue.Parameter != null))
                {
                    f_pAuthanticate(ParsedValue.Parameter);
                }
            }
        }
        private bool f_pAuthanticate(string p_CredentialValues)
        {
            bool l_bIsValid = false;
            try
            {
                var l_cCredential = Encoding.GetEncoding("iso-8859-1").GetString(Convert.FromBase64String(p_CredentialValues));
                var values = l_cCredential.Split(':');
                Result<List<SYSTEM_USER>> l_cSystemUser = new SYSTEM_USER().CheckUser(p_sEmail: values[0], p_sPassword: values[1]);
                if (l_cSystemUser.m_cDetail.m_eProcessState == ProcessState.Successful && l_cSystemUser.m_cData.Count > 0)
                {
                    l_bIsValid = true;
                    if (l_cSystemUser.m_cData[0].SYSTEM_USER_TYPE.SYSTEM_USER_TYPE_ID==1)
                        f_pSetPrinciPal(new GenericPrincipal(new GenericIdentity(values[0], "ADMIN"), null));
                    else if (l_cSystemUser.m_cData[0].SYSTEM_USER_TYPE.SYSTEM_USER_TYPE_ID == 2)
                        f_pSetPrinciPal(new GenericPrincipal(new GenericIdentity(values[0], "USER"), null));
                    else if (l_cSystemUser.m_cData[0].SYSTEM_USER_TYPE.SYSTEM_USER_TYPE_ID == 3)
                        f_pSetPrinciPal(new GenericPrincipal(new GenericIdentity(values[0], "OWNER"), null));
                    else if (l_cSystemUser.m_cData[0].SYSTEM_USER_TYPE.SYSTEM_USER_TYPE_ID == 4)
                        f_pSetPrinciPal(new GenericPrincipal(new GenericIdentity(values[0], "UNKNOWN"), null));
                }
                return l_bIsValid;
            }
            catch (Exception)
            {
                l_bIsValid = false;
            }
            return l_bIsValid;
        }
        private static void f_pSetPrinciPal(IPrincipal pricipal)
        {
            Thread.CurrentPrincipal = pricipal;
            if (HttpContext.Current != null)
                HttpContext.Current.User = pricipal;
        }
        public void OnLogRequest(Object source, EventArgs e)
        {
            //custom logging logic can go here
        }
    }
}
