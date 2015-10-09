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
namespace BitLySharp.Utilities
{
   
    internal class ParamList 
    {
        
        private List<KeyValuePair<string, string>> paramDic = new List<KeyValuePair<string, string>>();

        internal void Add(string key, string value)
        {
            paramDic.Add(new KeyValuePair<string, string>(key, value));
        }

        public override string ToString()
        {
            StringBuilder query = new StringBuilder();
            query.Append("?");
            foreach(KeyValuePair<string, string> pair in paramDic)
            {
                query.Append(String.Format("{0}={1}&", pair.Key, pair.Value));
            }
            query.Length--;
            return query.ToString();
        }
    }
}
