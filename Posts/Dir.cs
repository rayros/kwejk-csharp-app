using System;
using System.IO;
namespace Posts
{
	public class Dir
	{
	  static String posts_dir = "posts";
	  public static string Post (int id, string style)
	  {
	    CreateMainDir ();
	    CreateDirId (id);
	    CreateDirStyle (id, style);
	    return posts_dir + "/" + id + "/" + style;
	  }
	  private static void CreateDir(String path)
	  {
	    if (!Directory.Exists (path)) {
	      Directory.CreateDirectory (path);
	    }
	  }
	  private static void CreateMainDir(){
	    CreateDir (posts_dir);
	  }
	  public static void CreateDirId(int id){
	    CreateDir (posts_dir + "/" + id);
	  }
	  public static void CreateDirStyle (int id, string style) {
	    CreateDir (posts_dir + "/" + id + "/" + style);
	  }
	}
}

