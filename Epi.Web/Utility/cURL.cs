using System;
using System.IO;
using System.Net;
using System.Linq;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Epi.Web.MVC.Utility
{
	public class cURL
	{
		string method;
		System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string,string>> headers;
		string url;
		string pay_load;
		string name;
		string value;

		string content_type;

		public cURL (string p_method, string p_headers, string p_url, string p_pay_load, string p_name = null,
		string p_value = null, string p_content_type = "application/json")
		{
			this.headers = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string,string>> ();

			this.content_type = p_content_type;

			this.name = p_name;
			this.value = p_value;
			this.AllowRedirect = true;

			switch (p_method.ToUpper ()) 
			{
				case "PUT":
					this.method = "PUT";
					break;
				case "POST":
					this.method = "POST";
					break;
				case "DELETE":
					this.method = "DELETE";
					break;
				case "HEAD":
					this.method = "HEAD";
				break;					
				case "GET":
				default:
					this.method = "GET";
					break;
			}

			url = p_url;
			pay_load = p_pay_load;
			if (p_headers != null) 
			{
				string[] name_value_list = p_headers.Split ('|');

				foreach (string name_value in name_value_list) 
				{
					string[] n_v = name_value.Split (' ');
					this.headers.Add (new System.Collections.Generic.KeyValuePair<string,string> (n_v [0], n_v [1]));
				}

			}
		}


		public bool AllowRedirect { get; set; }

		public cURL AddHeader(string p_name, string p_value)
		{
			this.headers.Add(new System.Collections.Generic.KeyValuePair<string,string>(p_name, p_value));
			return this;
		}

		public string execute ()
		{
			string result = null;

			var httpWebRequest = (HttpWebRequest)WebRequest.Create(this.url);
			httpWebRequest.ReadWriteTimeout = 100000; //this can cause issues which is why we are manually setting this
			httpWebRequest.ContentType = this.content_type;
			httpWebRequest.PreAuthenticate = false;
			httpWebRequest.Accept = "*/*";
			httpWebRequest.Method = this.method;
			httpWebRequest.AllowAutoRedirect = this.AllowRedirect;

			if (!string.IsNullOrWhiteSpace(this.name) && !string.IsNullOrWhiteSpace(this.value))
			{
				string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(this.name + ":" + this.value));
				httpWebRequest.Headers.Add("Authorization", "Basic " + encoded);
			}


			foreach (System.Collections.Generic.KeyValuePair<string,string> kvp in this.headers) 
			{
				httpWebRequest.Headers.Add (kvp.Key, HeaderNameOrValueEncode(kvp.Value));
			}

			if (this.pay_load != null) 
			{
				//httpWebRequest.ContentLength = this.pay_load.Length;

				using (var streamWriter = new StreamWriter (httpWebRequest.GetRequestStream ())) 
				{
					streamWriter.Write (this.pay_load);
					streamWriter.Flush ();
					streamWriter.Close ();
				}
			}

			//try
			//{
				HttpWebResponse resp = (HttpWebResponse)httpWebRequest.GetResponse();
				result = new StreamReader(resp.GetResponseStream()).ReadToEnd();
				//Console.WriteLine("Response : " + respStr); // if you want see the output
			//}
			//catch(Exception ex)
			//{
				//process exception here   
			//	result = ex.ToString();
			//}

			return result;
		}


        public async System.Threading.Tasks.Task<string> executeAsync()
        {
            string result = null;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create (this.url);
            httpWebRequest.ReadWriteTimeout = 100000; //this can cause issues which is why we are manually setting this
            httpWebRequest.ContentType = this.content_type;
            httpWebRequest.PreAuthenticate = false;
            httpWebRequest.Accept = "*/*";
            httpWebRequest.Method = this.method;
            httpWebRequest.AllowAutoRedirect = this.AllowRedirect;

            if (!string.IsNullOrWhiteSpace (this.name) && !string.IsNullOrWhiteSpace (this.value))
			{
                string encoded = System.Convert.ToBase64String (System.Text.Encoding.GetEncoding ("ISO-8859-1").GetBytes (this.name + ":" + this.value));
                httpWebRequest.Headers.Add ("Authorization", "Basic " + encoded);
            }

			System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 -]");
            foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in this.headers) 
			{
				var key = rgx.Replace(kvp.Key, "");
				var val = rgx.Replace(kvp.Value, "");
				if(!string.IsNullOrWhiteSpace(key))
				{
					httpWebRequest.Headers.Add (key, HeaderNameOrValueEncode(val));
				}
                
            }

            if (this.pay_load != null) 
			{
                //httpWebRequest.ContentLength = this.pay_load.Length;

                using (var streamWriter = new StreamWriter (httpWebRequest.GetRequestStream ())) 
				{
                    streamWriter.Write (this.pay_load);
                    streamWriter.Flush ();
                    streamWriter.Close ();
                }
            }

            //try
            //{
            WebResponse resp = await httpWebRequest.GetResponseAsync ();
            result = new StreamReader (resp.GetResponseStream ()).ReadToEnd ();
            //Console.WriteLine("Response : " + respStr); // if you want see the output
            //}
            //catch(Exception ex)
            //{
            //process exception here   
            //  result = ex.ToString();
            //}

            return result;
        }


		public cURL add_authentication_header(string p_name, string p_value)
		{

			this.name = p_name;
			this.value = p_value;

			return this;
		}

		public static string HeaderNameOrValueEncode(string headerString)
		{
			if (string.IsNullOrEmpty(headerString))
			{
				return headerString;
			}
			else
			{
				var sb = new System.Text.StringBuilder();
				//headerString.All(ch => { if ((ch == 9 || ch >= 32) && ch != 127) sb.Append(ch); return true; });

				foreach(var ch in headerString)
				{
					if 
					(
						(ch == 9 || ch >= 32) && 
						ch != 127
					)
					{
						sb.Append(ch);
					}
				}
				
				return sb.ToString();
			}
		}   

	}
}

