using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /** 
    *<summary>Use this enum to select the grant_type, use <code>GratType.Password</code> in order to 
    *use username/password authentication, use <code>GrantType.Code</code> for the Web Flow OAuth</summary>
    */
    public enum GrantType { Password, Code}
    /**    
    *<summary>Class where are stored all the constants and enums that will be used in BitLySharp</summary>
    */
    public static class Constants
    {
        #region Api Urls
        public const string HOST = "https://api-ssl.bitly.com/";
        public const string AUTHORIZE_URL = "oauth/authorize";
        public const string ACCESS_TOKEN_URL = "oauth/access_token";
        public const string EXPAND_URL = "v3/expand";
        public const string LINK_INFO_URL = "v3/info";
        public const string LINK_LOOKUP = "v3/link/lookup";
        public const string LINK_SHORT_URL = "v3/shorten";
        public const string USER_LINK_EDIT_URL = "v3/user/link_edit";
        public const string USER_LINK_LOOKUP_URL = "v3/user/link_lookup";
        public const string USER_LINK_SAVE_URL = "v3/user/link_save";
        public const string USER_INFO = "v3/user/info";
        public const string USER_LINK_HISTORY = "v3/user/link_history";
        #endregion


    }
}
