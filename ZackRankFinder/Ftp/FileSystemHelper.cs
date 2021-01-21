using System.IO;

public class FileSystemHelper
{
    /// <summary>
    /// string p1 = "c:\\temp\\";
    /// string p2 = "\\subdir\\file\\";
    /// to c:\temp\subdir\file
    /// </summary>
    public static string CombineDirectory
              (string rootDirectoryPath, string childDirectoryPath)
    {
        rootDirectoryPath = rootDirectoryPath.TrimEnd('\\');
        childDirectoryPath = childDirectoryPath.Trim('\\');
        return Path.Combine(rootDirectoryPath, childDirectoryPath);
    }

    /// <summary>
    /// string p1 = "c:\\temp\\";
    /// string p2 = "\\file.text";
    /// to c:\temp\file.text
    /// </summary>
    public static string CombineFile(string rootDirectoryPath, string filePathOrName)
    {
        rootDirectoryPath = rootDirectoryPath.TrimEnd('\\');
        filePathOrName = filePathOrName.Trim('\\');
        return Path.Combine(rootDirectoryPath, filePathOrName);
    }

    public static void CreateDirectoryIfNotExists(string directoryPath)
    {
        if (!DirectoryExists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    public static void DeleteFileIfExists(string filePath)
    {
        if (FileExists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public static bool DirectoryExists(string directoryPath)
    {
        return Directory.Exists(directoryPath);
    }

    public static bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    /*file*/
    public static void MoveFile(string fromFilePath, string toFilePath)
    {
        File.Move(fromFilePath, toFilePath);
    }

    public static void FileAppendAllText(string filePath, string contents)
    {
        /*create file if doesn't exist and add line*/
        File.AppendAllText(filePath, contents);
    }
}