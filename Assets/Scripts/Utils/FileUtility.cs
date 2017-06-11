using System;
using System.IO;

public class FileUtility {
	public static void createFold (string activeDir, string subDir)
	{
		//Create a new subfolder under the current active folder
		string newPath = System.IO.Path.Combine (activeDir, subDir);

		// Determine whether the directory exists.
		if (!Directory.Exists (newPath)) {
			// Create the subfolder
			Directory.CreateDirectory (newPath);
		}
	}

	public static bool isErrorImage(UnityEngine.Texture2D tex) {
		//The "?" image that Unity returns for an invalid www.texture has these consistent properties:
		//(we also reject null.)
		return (tex.height == 8 && tex.width == 8);
	}

	public static bool CopyAndPasteFolder(string sPath, string dPath){
		return MoveFile(false, sPath, dPath);
	}

	/// <summary>
	/// Copy文件夹
	/// </summary>
	/// <param name="sPath">源文件夹路径</param>
	/// <param name="dPath">目的文件夹路径</param>
	/// <returns>完成状态：true-完成</returns>
	public static bool CutAndPasteFolder(string sPath, string dPath)
	{
		return MoveFile(true, sPath, dPath);
	}

	private static bool MoveFile(bool cutFile , string sPath, string dPath){
		bool flag = true;
		try {
			// 创建目的文件夹
			if (!Directory.Exists(dPath))
				Directory.CreateDirectory(dPath);
			
			// 拷贝文件
			DirectoryInfo sDir = new DirectoryInfo(sPath);
			FileInfo[] fileArray = sDir.GetFiles();
			if(fileArray != null)
			foreach (FileInfo file in fileArray) {
				file.CopyTo( System.IO.Path.Combine (dPath, file.Name), true);
				if(cutFile) file.Delete();
			}
			
			
			// 循环子文件夹
			DirectoryInfo dDir = new DirectoryInfo(dPath);
			DirectoryInfo[] subDirArray = sDir.GetDirectories();
			
			if(subDirArray != null) {
				foreach (DirectoryInfo subDir in subDirArray)
					CutAndPasteFolder( subDir.FullName, System.IO.Path.Combine(dPath,subDir.Name) );
			}
		}
		catch (Exception ex) {

			flag = false;
		}
		return flag;
	}

	// Append content to file
	public static void AppendFile(string filepath,string outdata) {

		try{
			using (StreamWriter sw = new StreamWriter( File.Open(filepath , FileMode.Append) )) {
				sw.Write (outdata);
			}	
		}catch(Exception e) {

		}

	}

}


