using System;
using Gtk;
namespace Project
{
	public partial class Dialog : Gtk.Dialog
	{
		public Project.Thread newThread = null;
		public string file_path = null;
		public Dialog (MainWindow main)
		{
			Build ();
			newThread = new Project.Thread (main);
		}

		protected void OnButton9Clicked (object sender, EventArgs e)
		{
			Gtk.FileChooserDialog fc=
				new Gtk.FileChooserDialog("Choose the file to open",
					this,
					FileChooserAction.Open,
					"Cancel",ResponseType.Cancel,
					"Open",ResponseType.Accept);

			if (fc.Run() == (int)ResponseType.Accept) 
			{
				file_path = fc.Filename;
				label3.Text = file_path;
				//Console.WriteLine (file_path);
			}
			fc.Destroy();
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			if(file_path != null)
			newThread.UploadPost (file_path, entry1.Text, textview1.Buffer.Text);
			this.Hide ();
			this.Destroy();
		}

		protected void OnButtonCancelClicked (object sender, EventArgs e)
		{
			this.Hide ();
			this.Destroy();
		}
	}
}

