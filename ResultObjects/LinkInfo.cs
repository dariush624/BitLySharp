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
    public class Info
    {
        [JsonProperty("title")]
        public object Title { get; set; }
        [JsonProperty("short_url")]
        public string ShortUrl { get; set; }
        [JsonProperty("created_at")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? CreatedAt { get; set; }
        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }
        [JsonProperty("global_hash")]
        public string GlobalHash { get; set; }
        [JsonProperty("user_hash")]
        public string UserHash { get; set; }
        /// <summary>
        /// If there were no errors, this property is null, otherwise is populated with a string which describes the kind of error. Example: "NOT_FOUND".
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; } = null;
    }

    public class Data
    {       
        [JsonProperty("info")]
        public List<Info> InfoList { get; set; }
    }

    public class LinkInfo
    {
        [JsonProperty("status_code")]
        public int StatusCode { get; set; }
        [JsonProperty("data")]
        public Data Data { get; set; }
        [JsonProperty("status_txt")]
        public string Status { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string JsonString { get; set; }
    }
}
