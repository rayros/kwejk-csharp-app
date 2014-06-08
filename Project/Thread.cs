using System;
using System.Threading;
using Posts;
using Gtk;
using System.Xml.Linq;

namespace Project
{
	public class Thread
	{
		MainWindow form;
		string xmlFile = "posts/waiting_room.xml";
		string file_path = null;
		string title = null;
		string desc = null;
		public Thread (MainWindow form)
		{
			this.form = form;
		}
		public void DownloadPosts(string xmlFile)
		{
			this.xmlFile = xmlFile;
			System.Threading.Thread download = new System.Threading.Thread (_Download);
			download.Start ();
		}
		public void UploadPost(string file_path, string title, string desc)
		{
			this.file_path = file_path;
			this.title = title;
			this.desc = desc;
			System.Threading.Thread upload = new System.Threading.Thread (_Upload);
			upload.Start ();
		}
		private void _Download()
		{
			XDocument doc = Download.getXml (xmlFile);
			Gtk.Application.Invoke (delegate {
				form.LoadImages (doc);
			});
		}
		private void _Upload()
		{
			Upload.Post (file_path, title, desc);
			_Download ();
		}
	}
}

