using System;
using System.IO;
using Gtk;
using Gdk;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Xml.Linq;
using Posts;
public partial class MainWindow: Gtk.Window
{
	public VBox posts = null;
	public Project.Thread newThread = null; 
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		this.AddEvents ((int)Gdk.EventMask.KeyPressMask);
		newThread = new Project.Thread (this);
		newThread.DownloadPosts ("posts.xml");
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
	public void LoadImages(XDocument doc)
	{
		if (posts != null) {
			foreach(VBox post in posts.Children){
				foreach (Widget obj in post.Children) {
					post.Remove (obj);
					obj.Destroy ();
				}
				posts.Remove (post);
				post.Destroy ();
			}
		}
		posts = new VBox ();
		vbox3.PackStart (posts,true,true,1);
		IEnumerable<XElement> posts_list = doc.XPathSelectElements("/posts/post");
		foreach (XElement elem in posts_list) {
			VBox post = new VBox ();
			string title = elem.XPathSelectElement ("title").Value;
			string description = elem.XPathSelectElement ("description").Value;
			post.PackStart (new Label (title), true, true, 1);
			IEnumerable<XElement> urls_list = elem.XPathSelectElements ("img/url");
			foreach (XElement urls in urls_list) {
				Gtk.EventBox eventBox = new Gtk.EventBox();
				string file_img_default = urls.XPathSelectElement ("default").Value;
				string file_img_original = urls.XPathSelectElement ("original").Value;
				Gtk.Image new_img = new Gtk.Image (	file_img_default);
				eventBox.Add (new_img);
				eventBox.ButtonPressEvent += delegate {
					new Project.ImgWindow (title, description, file_img_original);
				};
				post.PackEnd (eventBox, true, true, 1);
			}
			posts.PackStart (post, true, true, 1);
		}
		posts_list = null;
		ShowAll ();

	}

	protected void OnWaitingRoomAction1Activated (object sender, EventArgs e)
	{

		newThread.DownloadPosts ("posts/waiting_room.xml");
	}

	protected void OnHomeActionActivated (object sender, EventArgs e)
	{
		newThread.DownloadPosts ("posts.xml");
	}

	protected void OnExitAction1Activated (object sender, EventArgs e)
	{
		HideAll ();
		this.OnDeleteEvent (this, new DeleteEventArgs());
	
	}
	protected void OnTopActionActivated (object sender, EventArgs e)
	{
		newThread.DownloadPosts ("posts/top.xml");
	}
	protected void OnHotnessActionActivated (object sender, EventArgs e)
	{
		newThread.DownloadPosts ("posts/hotness.xml");
	}
	protected void OnUploadAction1Activated (object sender, EventArgs e)
	{
		new Project.Dialog (this);
	}

	[GLib.ConnectBefore]
	protected override bool OnKeyPressEvent (Gdk.EventKey e) {
		if (e.Key == Gdk.Key.Escape) {
			this.OnDeleteEvent (this, new DeleteEventArgs());
			HideAll ();
		};
		return base.OnKeyPressEvent (e);
	}

}
