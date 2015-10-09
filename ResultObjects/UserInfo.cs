using BitLySharp.Utilities;
using Newtonsoft.Json;
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
namespace BitLySharp.ResultObjects
{
    public class UserInfoData
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }
        [JsonProperty("custom_short_domain")]
        public object CustomShortDomain { get; set; }
        [JsonProperty("display_name")]
        public object DisplayName { get; set; }
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        [JsonProperty("in_enterprise")]
        public bool IsEnterprise { get; set; }
        [JsonProperty("login")]
        public string Login { get; set; }
        [JsonProperty("member_since")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? MemberSince { get; set; }
        [JsonProperty("profile_image")]
        public string ProfileImage { get; set; }
        [JsonProperty("profile_url")]
        public string ProfileUrl { get; set; }
        [JsonProperty("share_accounts")]
        public List<object> ShareAccounts { get; set; }
        [JsonProperty("tracking_domains")]
        public List<object> TrackingDomains { get; set; }
    }

    public class UserInfo
    {
        [JsonProperty("data")]
        public UserInfoData Data { get; set; }
        [JsonProperty("status_code")]
        public int StatusCode { get; set; }
        [JsonProperty("status_txt")]
        public string Status { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string JsonString { get; set; }
    }
}
