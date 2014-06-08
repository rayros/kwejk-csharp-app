using System;
using Gtk;
namespace Project
{
	public partial class ImgWindow : Gtk.Window
	{
		public ImgWindow (string title, string description, string fileName) : 
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
			label2.Text = title;
			Label a = new Label(description);
			Gtk.Image new_img = new Gtk.Image (fileName);
			vbox3.PackEnd (a, true, true, 1);
			vbox3.PackEnd (new_img, true, true, 1);
			ShowAll ();
		}
	}
}

