using BitLySharp.Exceptions;
using BitLySharp.ResultObjects;
using BitLySharp.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
/*
The MIT License (MIT)

Copyright (c) 2015 Dariush Moshiri

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
namespace BitLySharp.Core
{
    /// <summary>
    /// Main Class used to handle API requests
    /// </summary>
    public class BitLy
    {
        private HttpClient client = new HttpClient();
        private GrantType grantType = GrantType.Code;
        private string username = "";
        private string password = "";
        private string redirectUri;

        #region Constructors
        /**
        *<summary>Constructor of <see cref="BitLySharp"/>, it takes two parameter</summary>
        *<param name="clientId">The Client ID of the application</param> 
        *<param name="clientSecret">The Client Secret of the application</param>
        *<exception cref="ArgumentException">If client id or client secret is null <see cref="ArgumentException"/> will be thrown</exception>
        */
        public BitLy(string clientId, string clientSecret)
        {
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            if (!String.IsNullOrEmpty(clientId) && !String.IsNullOrEmpty(clientSecret))
            {
                ClientCredential.ClientId = clientId;
                ClientCredential.ClientSecret = clientSecret;
            } else
            {
                throw new ArgumentException("Client ID and Client Secret cannot be null or empty");
            }
        }

        /**
        *<summary>Constructor of <see cref="BitLySharp"/>, it takes two parameter</summary>
        *<param name="clientId">The Client ID of the application</param> 
        *<param name="clientSecret">The Client Secret of the application</param>
        *<param name="redirect">The redirect url of your app</param>
        *<exception cref="ArgumentException">If client id or client secret is null <see cref="ArgumentException"/> will be thrown</exception>
        */
        public BitLy(string clientId, string clientSecret, string redirect) : this(clientId, clientSecret)
        {
            this.redirectUri = redirect;          
        }

        /**
        *<summary>Constructor of <see cref="BitLySharp"/>, use this one if you want to set the grant_type to password</summary>
        *<param name="clientId">The Client ID of the application</param> 
        *<param name="clientSecret">The Client Secret of the application</param>
        *<param name="type">Grant Type using during authentication</param>
        *<param name="pass">Password of the user</param>
        *<param name="user">Username</param>
        *<exception cref="ArgumentException">If client id or client secret is null <see cref="ArgumentException"/> will be thrown</exception>
        */
        public BitLy(string clientId, string clientSecret, GrantType type, string user, string pass) : this(clientId, clientSecret)
        {
            grantType = type;
            if(type == GrantType.Password)
            {
                username = user;
                password = pass;
            }      
           
        }
        #endregion

        #region OAuth Methods
        /**     
        *<summary><remarks>If your grant_type is password use the <see cref="GetAccessTokenAsync(string)"/> method</remarks>. This method use <typeparamref name="WebAuthenticationBroker"/> in order to allow the user to insert his/her credentials on bit.ly website</summary>
        *<returns>Returns the <see cref="AuthenticationInfo"/> object which stores the Access Token  and the Login key (YOU NEED TO STORE THIS TOKEN BECAUSE IT IS USED WITH EVERY API REQUEST)</returns>
        */
        public async Task<AuthenticationInfo> AuthorizeAsync()
        {
            if(grantType == GrantType.Code)
            {
                Uri authorizeUrl = new Uri(String.Format("https://bitly.com/" + Constants.AUTHORIZE_URL + "?client_id={0}&redirect_uri={1}", ClientCredential.ClientId, redirectUri));
                WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, authorizeUrl, new Uri(redirectUri));
                if(result.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    string code = GetCodeFromAuthResult(result.ResponseData.ToString());
                    AuthenticationInfo accessToken = await GetAccessTokenAsync(code);
                    return accessToken;
                } else
                {
                    throw new WebAuthenticationException("Error during authentication details: " + result.ResponseErrorDetail.ToString());
                }
            } else
            {
                throw new WebAuthenticationException("You cannot use AuthorizeAsync() if grant_type is password");
            }
        }

        /**
        *<summary>This method gets the access token from bit.ly either via <paramref name="code"/> or via username/password depending on grant_type</summary>
        *<param name="code">Used to get the access token with Web Flow OAuth</param>
        *<returns>Returns an <see cref="AuthenticationInfo"/> object which stores Access Token and Login (if Web Flow OAuth) or just the access token (username/password OAuth)</returns>
        */
        public async Task<AuthenticationInfo> GetAccessTokenAsync(string code = "")
        {
           if(grantType == GrantType.Code)
            {
                Uri url = new Uri(String.Format(Constants.HOST + Constants.ACCESS_TOKEN_URL + "?client_id={0}&client_secret={1}&code={2}&redirect_uri={3}", ClientCredential.ClientId, ClientCredential.ClientSecret, code, redirectUri));                
                var response = await client.PostAsync(url, new StringContent(""));
                response.EnsureSuccessStatusCode();
                AuthenticationInfo info = GetAuthenticationInfoFromResponse(await response.Content.ReadAsStringAsync());
                return info;
            } else
            {
                Uri url = new Uri(String.Format(Constants.HOST + Constants.ACCESS_TOKEN_URL + "?grant_type=password&username={0}&password={1}", username, password));
                string authBasic = Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format("{0}:{1}", ClientCredential.ClientId, ClientCredential.ClientSecret)));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + authBasic);
                var response = await client.PostAsync(url, new StringContent(""));
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                JsonSerializerSettings settings = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore };
                AuthenticationInfo info = JsonConvert.DeserializeObject<AuthenticationInfo>(json, settings);
                return info;
            }
        }
        #endregion

        #region Api Methods
        #region Link Api
        /**
        *<summary>This method gets the exapanded Url given a bit.ly short url</summary>
        *<param name="accessToken">Access token of the user, obtained via <see cref="AuthorizeAsync"/> or <see cref="GetAccessTokenAsync(string)"/></param>
        *<param name="shortUrl">Short url not yet escaped</param>
        *<returns>Returns the Long Url</returns>
        */
        public async Task<string> ExpandUrlAsync(string accessToken, string shortUrl)
        {
            string escapedShortUrl = Uri.EscapeUriString(shortUrl);
            ParamList plist = new ParamList();
            plist.Add("access_token", accessToken);
            plist.Add("shortUrl", escapedShortUrl);
            plist.Add("format", "txt");
            Uri url = new Uri(Constants.HOST + Constants.EXPAND_URL + plist.ToString());
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string longUrl = await response.Content.ReadAsStringAsync();
            return longUrl;
        }

        /**
        *<summary>This method gets the information about a short Url</summary>
        *<param name="accessToken">Access token of the user, obtained via <see cref="AuthorizeAsync"/> or <see cref="GetAccessTokenAsync(string)"/></param>
        *<param name="shortUrls">Short urls not yet escaped</param>
        *<returns>Returns <see cref="LinkInfo"/> object, use <see cref="LinkInfo.JsonString"/> to get the json string</returns>
        */
        public async Task<LinkInfo> GetUrlInfoAsync(string accessToken, params string[] shortUrls)
        {
            List<string> escapedShortUrls = EscapeUrls(shortUrls);

            ParamList plist = new ParamList();
            plist.Add("access_token", accessToken);

            foreach (string escapedUrl in escapedShortUrls)
                plist.Add("shortUrl", escapedUrl);
                     
            Uri url = new Uri(Constants.HOST + Constants.LINK_INFO_URL + plist.ToString());
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            JsonSerializerSettings settings = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore, DateParseHandling = DateParseHandling.DateTime };
            LinkInfo info = JsonConvert.DeserializeObject<LinkInfo>(json, settings);
            info.JsonString = json;
            return info;
        }

        /**
        *<summary>This is used to query for a Bitlink based on a long URL</summary>
        *<param name="accessToken">Access token of the user, obtained via <see cref="AuthorizeAsync"/> or <see cref="GetAccessTokenAsync(string)"/></param>
        *<param name="urls">Long urls not yet escaped</param>
        *<returns>Returns a <see cref="LinkLookupInfo"/> object, use <see cref="LinkLookupInfo.JsonString"/> to get the json string</returns>
        */
        public async Task<LinkLookupInfo> LinkLookupAsync(string accessToken, params string[] urls)
        {
            List<string> escapedUrls = EscapeUrls(urls);
            ParamList pList = new ParamList();
            pList.Add("access_token", accessToken);

            foreach (string escapedUrl in escapedUrls)
                pList.Add("url", escapedUrl);

            Uri url = new Uri(Constants.HOST + Constants.LINK_LOOKUP + pList.ToString());
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            JsonSerializerSettings settings = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore };
            LinkLookupInfo info = JsonConvert.DeserializeObject<LinkLookupInfo>(json, settings);
            info.JsonString = json;

            return info;
        }

        /// <summary>
        /// Given a long URL, returns a Bitlink.
        /// </summary>
        /// <param name="accessToken">Access token of the user, obtained via <see cref="AuthorizeAsync"/> or <see cref="GetAccessTokenAsync(string)"/></param>
        /// <param name="url">Long url</param>
        /// <returns>The bitlink url as <see cref="string"/></returns>
        public async Task<string> ShortenUrlAsync(string accessToken, string url)
        {
            string escapedUrl = Uri.EscapeUriString(url);
            ParamList pList = new ParamList();
            pList.Add("access_token", accessToken);
            pList.Add("url", escapedUrl);
            pList.Add("format", "txt");

            Uri apiUrl = new Uri(Constants.HOST + Constants.LINK_SHORT_URL + pList.ToString());
            var response = await client.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            string shortUrl = await response.Content.ReadAsStringAsync();

            return shortUrl;
        }

        /// <summary>
        /// Edit information about a link
        /// </summary>
        /// <param name="accessToken">Access token of the user, obtained via <see cref="AuthorizeAsync"/> or <see cref="GetAccessTokenAsync(string)"/></param>
        /// <param name="link">Short Url (Bitlink) to edit</param>
        /// <param name="title">The title of your bitlink</param>
        /// <param name="note">Notes</param>
        /// <param name="privat"><code>true</code> if you want the bitlink to be private or <code>false</code> to be public</param>
        /// <param name="userTimeStamp">Change the timestamp</param>
        /// <param name="archived"><code>true</code> if you want the bitlink to be archived false otherwise</param>
        /// <returns></returns>
        public async Task<LinkEditResponseInfo> EditLinkAsync(string accessToken, string link, string title = null, string note = null, bool? privat = null, DateTime? userTimeStamp = null, bool? archived = null)
        {
            string escapedLink = Uri.EscapeUriString(link);

            ParamList pList = new ParamList();
            pList.Add("access_token", accessToken);
            pList.Add("link", escapedLink);
            StringBuilder edit = new StringBuilder();

            if (title != null)
            {
                pList.Add("title", Uri.EscapeDataString(title));
                edit.Append("title,");
            }
            if(note != null)
            {
                pList.Add("note", Uri.EscapeDataString(note));
                edit.Append("note,");
            }

            if (privat != null)
            {
                pList.Add("private", privat.Value.ToString().ToLower());
                edit.Append("private,");
            }

            if(userTimeStamp != null)
            {
                DateTime? epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                double user_ts = Math.Ceiling((userTimeStamp - epoch).Value.TotalSeconds);
                pList.Add("user_ts", user_ts.ToString());
                edit.Append("user_ts,");
            }

            if (archived != null)
            {
                pList.Add("archived", archived.Value.ToString().ToLower());
                edit.Append("archived,");
            }

            if (edit.Length != 0)
            {
                edit.Length--;
                pList.Add("edit", edit.ToString());
            }

            Uri url = new Uri(Constants.HOST + Constants.USER_LINK_EDIT_URL + pList.ToString());
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            LinkEditResponseInfo info = JsonConvert.DeserializeObject<LinkEditResponseInfo>(json, new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore });
            info.JsonString = json;

            return info;

        }

        /// <summary>
        /// This is used to query for a Bitlink shortened by the authenticated user based on a long URL or a Bitlink. <paramref name="longUrls"/> and <paramref name="shortUrls"/> cannot be set both at the same time
        /// </summary>
        /// <param name="accessToken">Access token of the user, obtained via <see cref="AuthorizeAsync"/> or <see cref="GetAccessTokenAsync(string)"/></param>
        /// <param name="longUrls">Extended urls</param>
        /// <param name="shortUrls">Bitlink urls</param>
        /// <returns>Return a <see cref="UserLinkLookupInfo"/> object, see the <see cref="UserLinkLookupInfo.JsonString"/> for the json string</returns>
        public async Task<UserLinkLookupInfo> LookupUserLinkAsync(string accessToken, string[] longUrls = null, string[] shortUrls = null)
        {
            if((longUrls == null) && (shortUrls == null))
            {
                throw new ArgumentException("Either longUrls or shortUrls must be set");
            } else if((longUrls != null) && (shortUrls != null))
            {
                throw new ArgumentException("longUrls and shortUrls cannot be different from null at the same time, LookupUserLinkAsync operate either on longUrls or shortUrls");
            } else
            {
                List<string> escapedUrls = (longUrls != null) ? EscapeUrls(longUrls) : EscapeUrls(shortUrls);

                ParamList pList = new ParamList();
                pList.Add("access_token", accessToken);

                foreach (string escapedUrl in escapedUrls)
                    pList.Add("url", escapedUrl);

                Uri url = new Uri(Constants.HOST + Constants.USER_LINK_LOOKUP_URL + pList.ToString());
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();

                UserLinkLookupInfo info = JsonConvert.DeserializeObject<UserLinkLookupInfo>(json, new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore });
                info.JsonString = json;

                return info;
            }
        }

        /// <summary>
        /// Saves a long URL as a Bitlink in a user's history, with optional pre-set metadata. (Also returns a short URL for that link.)
        /// </summary>
        /// <param name="accessToken">Access token of the user, obtained via <see cref="AuthorizeAsync"/> or <see cref="GetAccessTokenAsync(string)"/></param>
        /// <param name="longUrl">Long url</param>
        /// <param name="title">Title of the url</param>
        /// <param name="note">Notes for the url</param>
        /// <param name="privat">Set true if private, false if public</param>
        /// <param name="userTimeStamp">Timestamp</param>
        /// <returns></returns>
        public async Task<UserLinkSavedInfo> SaveLinkAsync(string accessToken, string longUrl, string title = null, string note = null, bool? privat = null, DateTime? userTimeStamp = null)
        {
            string escapedUrl = Uri.EscapeUriString(longUrl);

            ParamList pList = new ParamList();
            pList.Add("access_token", accessToken);
            pList.Add("longUrl", longUrl);

            if (title != null)
                pList.Add("title", Uri.EscapeDataString(title));
            if (note != null)
                pList.Add("note", Uri.EscapeDataString(note));
            if (privat != null)
                pList.Add("private", privat.Value.ToString().ToLower());
            if(userTimeStamp != null)
            {
                DateTime? epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                double user_ts = Math.Ceiling((userTimeStamp - epoch).Value.TotalSeconds);
                pList.Add("user_ts", user_ts.ToString());
            }

            Uri url = new Uri(Constants.HOST + Constants.USER_LINK_SAVE_URL + pList.ToString());
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            UserLinkSavedInfo info = JsonConvert.DeserializeObject<UserLinkSavedInfo>(json, new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore });
            info.JsonString = json;

            return info;

        }
        #endregion

        #region User Info
        /// <summary>
        /// Get information the authenticated user or the user specified by the login parameter
        /// </summary>
        /// <param name="accessToken">Access Token of the authenticated user</param>
        /// <param name="login">Bitly login to get the user information, if null authenticated user's info will be got</param>
        /// <param name="fullname">If null nothing happens, otherwise (if the user ia authenticated) his/her Full Name will be changed</param>
        /// <returns><see cref="UserInfo"/> object (see the <see cref="UserInfo.JsonString"/> property to get the JSON string)</returns>
        public async Task<UserInfo> GetUserInfoAsync(string accessToken, string login = null, string fullname = null)
        {
            ParamList list = new ParamList();
            list.Add("access_token", accessToken);
            if (login != null)
                list.Add("login", login);
            if (fullname != null)
                list.Add("full_name", fullname);

            Uri url = new Uri(Constants.HOST + Constants.USER_INFO + list.ToString());
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            UserInfo user = JsonConvert.DeserializeObject<UserInfo>(json, new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore });
            user.JsonString = json;

            return user;
        }
        /// <summary>
        /// Get the user's links history
        /// </summary>
        /// <param name="accessToken">Access token of the athenticated user</param>
        /// <param name="link">Bitlink to return metadata for</param>
        /// <param name="limit">Integer in the range 1 to 100, specifying the max number of results to return</param>
        /// <param name="offset">Integer specifying the numbered result at which to start</param>
        /// <param name="createdBefore">Get all the links created before this date</param>
        /// <param name="createdAfter">Get all the links created after this date</param>
        /// <param name="modifiedAfter">Get all the links modified after this date</param>
        /// <returns></returns>
        public async Task<UserLinkHistory> GetUserLinkHistoryAsync(string accessToken, string link = null, int limit = 50, int offset = 0, DateTime? createdBefore = null, DateTime? createdAfter = null, DateTime? modifiedAfter = null)
        {
            ParamList list = new ParamList();
            list.Add("access_token", accessToken);
            if (link != null)
                list.Add("link", Uri.EscapeUriString(link));
            if (createdBefore != null)
                list.Add("created_before", GetEpochFromDateTime(createdBefore).ToString());
            if (createdAfter != null)
                list.Add("created_after", GetEpochFromDateTime(createdAfter).ToString());
            if (modifiedAfter != null)
                list.Add("created_after", GetEpochFromDateTime(modifiedAfter).ToString());

            if (limit < 1 || limit > 100)
                throw new ArgumentException("The limit parameter must be between 1 and 100");
            
            list.Add("limit", limit.ToString());
            list.Add("offset", offset.ToString());

            Uri url = new Uri(Constants.HOST + Constants.USER_LINK_HISTORY + list.ToString());
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            UserLinkHistory linkhistory = JsonConvert.DeserializeObject<UserLinkHistory>(json, new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore });
            linkhistory.JsonString = json;

            return linkhistory;
        }
        #endregion

        #endregion

        #region Utilities Methods

        private List<string> EscapeUrls(string[] urls)
        {
            List<string> esUrls = new List<string>();
            foreach (string url in urls)
                esUrls.Add(Uri.EscapeUriString(url));
            return esUrls;
        }

        private long GetEpochFromDateTime(DateTime? datetime)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((datetime.Value - epoch).TotalSeconds);
        }

        private AuthenticationInfo GetAuthenticationInfoFromResponse(string response)
        {
            string[] parts = response.Split('&');
            string token = parts[0].Split('=')[1];
            string login = parts[1].Split('=')[1];
            return new AuthenticationInfo() { AccessToken = token, Login = login };
        }

        

        private string GetCodeFromAuthResult(string url)
        {
            string[] urlParts = url.Replace('?', ' ').Replace('&', ' ').Split(' ');
            string c = String.Empty;
            for (int i = 0; i < urlParts.Length; i++)
            {
                if (urlParts[i].Contains("code"))
                {
                    c = urlParts[i].Split('=')[1];
                }
            }
            return c;
        }
        #endregion
    }
}
