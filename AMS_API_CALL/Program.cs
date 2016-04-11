using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using System.Runtime.Serialization;
//using System.Runtime.Serialization.Json;
//using System.Web.Script.Serialization;
using System.Text;
//using System.Data.OracleClient;
using System.Data;

namespace AMS_API_CALL
{

    class Program
    {
        public String streamcontent { get; set; }
        public String download_req_status { get; set; }
        static void Main()
        {

            Program p = new Program();
            p.Get_Status_Only();
          
            Console.ReadKey();

        }
        
        public  String Get_Status_Only()
        {
            List<String> IDs = new List<String>();
            try

            {
               
                var builder = new UriBuilder("http://206.202.171.237/ams/lib/php/manage-media-data.php?func=list");
                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(builder.Query);
                queryString["location_id"] = "2";
                builder.Query = queryString.ToString();
                string url = builder.ToString();
                streamcontent = sendRequest(url);
                var doc = XDocument.Parse(streamcontent);
                var result = from row in doc.Root.Descendants("archive_media_file")
                             where Convert.ToInt64(row.Element("media_file_start").Value) == 1452520800
                             && Convert.ToInt64(row.Element("media_file_end").Value) == 1452521700 
                            && Convert.ToInt64( row.Element("media_file_added").Value) == 1455911166

                             //  && Convert.ToString(row.Element("media_file_sys_id").Value) == Convert.ToString(requestData[2])
                             select new XElement("archive_media_file", row.Element("media_file_status"));

                if (result != null)
                {
                    // download_req_status = result.ToString();
                    foreach (XElement element in result.Descendants("media_file_status"))
                    {
                        download_req_status = element.Value.Trim();
                        Console.WriteLine(download_req_status);
                    }
                }
               
            }
            catch
            {

            }
            return null;

        }

        public String sendRequest(String url)
        {

          
            try
            {
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                WebResponse response = myHttpWebRequest.GetResponse();
                Stream stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                streamcontent = sr.ReadToEnd();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return streamcontent;
        }

        public static List<String> readStreamContent(List<String> xmlnodes, String streamcontent)
        {
            List<String> xmlValue = new List<String>();

            var doc = XDocument.Parse(streamcontent);

          //  var result= from row in doc select
            foreach (XElement element in doc.Descendants(Convert.ToString(xmlnodes[0])))
            {
                Console.WriteLine(element.Value.Trim());
                xmlValue.Add(element.Value.Trim());

                foreach (XElement element1 in doc.Descendants(Convert.ToString(xmlnodes[1])))
                {

                    Console.WriteLine(element1.Value.Trim());
                    xmlValue.Add(element1.Value.Trim());

                    foreach(XElement element2 in doc.Descendants(Convert.ToString(xmlnodes[2])))
                    {
                        Console.WriteLine(element2.Value.Trim());
                        xmlValue.Add(element2.Value.Trim());
                    }
                }

            }
            Console.WriteLine(xmlValue);

            return xmlValue;
        }

        
    }
 }

