
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;

namespace Posts
{
	public class Upload {
	  public static void Post(string file, string title, string description)
	  {
	    NameValueCollection nvc = new NameValueCollection();
	    nvc.Add("post[title]", title);
	    nvc.Add("post[description]", description);
	    nvc.Add("commit", "Save");
	    HttpUploadFile("http://empatia.herokuapp.com/posts",  file, "post[picture]", GetContentType(file), nvc);
	  }
	  private static string GetContentType(string fileName)
	  {
	    string ext = System.IO.Path.GetExtension(fileName).ToLower();
	    ext = ext.Substring(1);
	    if ( ext == "jpg") ext = "jpeg";
	    return "image/"+ ext;
	  }

	  private static void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc) {
	    Console.WriteLine("Uploading {0} to {1}", file, url);
	    string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
	    byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
	    try {
	      HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
	      wr.ContentType = "multipart/form-data; boundary=" + boundary;
	      wr.Method = "POST";
	      wr.KeepAlive = true;
	      wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

	      Stream rs = wr.GetRequestStream();

	      string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
	      foreach (string key in nvc.Keys)
	      {
	        rs.Write(boundarybytes, 0, boundarybytes.Length);
	        string formitem = string.Format(formdataTemplate, key, nvc[key]);
	        byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
	        rs.Write(formitembytes, 0, formitembytes.Length);
	      }
	      rs.Write(boundarybytes, 0, boundarybytes.Length);

	      string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
	      string header = string.Format(headerTemplate, paramName, file, contentType);
	      byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
	      rs.Write(headerbytes, 0, headerbytes.Length);

	      FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
	      byte[] buffer = new byte[4096];
	      int bytesRead = 0;
	      while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0) {
	          rs.Write(buffer, 0, bytesRead);
	      }
	      fileStream.Close();

	      byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
	      rs.Write(trailer, 0, trailer.Length);
	      rs.Close();

	      WebResponse wresp = null;
	      try {
	        wresp = wr.GetResponse();
	        Stream stream2 = wresp.GetResponseStream();
	        StreamReader reader2 = new StreamReader(stream2);
	        Console.WriteLine("File uploaded, server response is: {0}", reader2.ReadToEnd());
	      } catch(Exception ex) {
	        Console.WriteLine("Error uploading file : {0}", ex);
	        if(wresp != null) {
	            wresp.Close();
	            wresp = null;
	        }
	      } finally {
	        wr = null;
	      }
	    }
	    catch(Exception ex)
	    {
	      Console.WriteLine("Exception connection url: {0}" , ex);
	    }
	  }
	}
}			