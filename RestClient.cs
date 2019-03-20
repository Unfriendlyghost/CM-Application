using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace App_Dev_A2_Client_Calendar
{
    public enum httpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    class RestClient
    {
        public string endPoint { get; set; }
        public httpVerb httpMethod { get; set; }
        public string postJSON { get; set; }
        public string putJSON { get; set; }
        public string deleteJSON { get; set; }

        public RestClient()
        {
            endPoint = string.Empty;
            httpMethod = httpVerb.GET;
        }

        public string makeRequest()
        {
            string strResponseValue = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);
            request.ContentType = "application/json";

            request.Method = httpMethod.ToString();

            if (request.Method == "POST" && postJSON != string.Empty)
            {
                using (StreamWriter swJSONPayload = new StreamWriter(request.GetRequestStream()))
                {
                    swJSONPayload.Write(postJSON);

                    swJSONPayload.Close();
                }
            }
            else if (request.Method == "PUT" && putJSON != string.Empty)
            {
                using (StreamWriter swJSONPayload = new StreamWriter(request.GetRequestStream()))
                {
                    swJSONPayload.Write(putJSON);

                    swJSONPayload.Close();
                }
            }
            else if (request.Method == "DELETE" && deleteJSON == string.Empty)
            {
                using (StreamWriter swJSONPayload = new StreamWriter(request.GetRequestStream()))
                {
                    swJSONPayload.Write(deleteJSON);

                    swJSONPayload.Close();
                }
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException("error code: " + response.StatusCode.ToString());
                }
                //Process the response stream... (could be JSON, XML, or HTML)

                using (Stream responseStream = response.GetResponseStream())
                {
                    if(responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                            strResponseValue = strResponseValue.Substring(1, strResponseValue.Length - 2);
                        }//End of StreamReader
                    }
                }//End of responseStream

            }//End of using response

            return strResponseValue;
        }
    }
}
