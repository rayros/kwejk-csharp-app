using System;
using System.Net;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Collections.Generic;
namespace Posts
{
	public class Download
	{
	  public static XDocument getXml(string xmlFile)
	  {
	    string url = "http://empatia.herokuapp.com/"+xmlFile;
	    string xml;
	    using (var webClient = new WebClient())
	    {
	        xml = webClient.DownloadString(url);
	    }
	    XDocument doc = XDocument.Parse(xml);
	    IEnumerable<XElement> posts_list = doc.XPathSelectElements("/posts/post");
	    foreach (XElement elem in posts_list)
	    {    
	      int id = Int32.Parse(elem.XPathSelectElement("id").Value);
	      string nameFile = elem.XPathSelectElement("img/file/name").Value;
	      XElement img_urls = elem.XPathSelectElement("img/url");
	      foreach (XElement url_style in img_urls.Elements())
	      {
	        string style = url_style.Name.ToString();
	        string img_url = url_style.Value;
	        url_style.Value = Image(id, style, nameFile, img_url);
	      }
	    }
		Console.WriteLine ("Xml downloading: success");
	    return doc;  
	  }  
	  private static string Image (int id, string style, string nameFile,  string url)
	  {
	    string dir = Dir.Post(id,style);
	    string path = dir + "/" + nameFile;
	    if(!File.Exists(path)) DownloadRemoteImageFile(url, path); 
	    return path;
	  }
	  /*
	   * @param input - www url
	   * @param output - file address
	   */
	  private static void DownloadRemoteImageFile(string input, string output) {
	    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(input);
	    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
	    if ((response.StatusCode == HttpStatusCode.OK ||
	        response.StatusCode == HttpStatusCode.Moved ||
	        response.StatusCode == HttpStatusCode.Redirect) &&
	        response.ContentType.StartsWith ("image", StringComparison.OrdinalIgnoreCase)) {

	      using (Stream inputStream = response.GetResponseStream())
	      using (Stream outputStream = File.OpenWrite(output))
	      {
	        byte[] buffer = new byte[4096];
	        int bytesRead;
	        do
	        {
	          bytesRead = inputStream.Read(buffer, 0, buffer.Length);
	          outputStream.Write(buffer, 0, bytesRead);
	        } while (bytesRead != 0);
	      }
	      Console.WriteLine ("Download file: "+input+ " success. :)");
	    }
	  }	
	}
}
