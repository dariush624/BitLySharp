﻿using BitLySharp.Utilities;
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
    public class LinkHistoryInfo
    {
        [JsonProperty("aggregate_link")]
        public string AggregateLink { get; set; }
        [JsonProperty("archived")]
        public bool Archived { get; set; }
        [JsonProperty("client_id")]
        public string ClientId { get; set; }
        [JsonProperty("created_at")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? CreatedAt { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        [JsonProperty("long_url")]
        public string LongUrl { get; set; }
        [JsonProperty("modified_at")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? ModifiedAt { get; set; }
        [JsonProperty("private")]
        public bool Private { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("user_ts")]
        public int UserTimestamp { get; set; }
    }

    public class LinkHistory
    {
        [JsonProperty("link_history")]
        public List<LinkHistoryInfo> LinkHistoryList { get; set; }
        [JsonProperty("result_count")]
        public int ResulCount { get; set; }
    }

    public class UserLinkHistory
    {
        [JsonProperty("status_code")]
        public int StatusCode { get; set; }
        [JsonProperty("data")]
        public LinkHistory Data { get; set; }
        [JsonProperty("status_txt")]
        public string Status { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string JsonString { get; set; }
    }
}
