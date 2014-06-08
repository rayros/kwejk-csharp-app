using System;
using Gtk;
using Posts;
using System.Xml;
using System.Xml.Linq;
namespace Project
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();

		}
	}
}
