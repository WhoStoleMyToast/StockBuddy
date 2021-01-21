using System;

public class FtpHelper
{
    /// <summary>
    /// string p1 = "/temp";
    /// to /temp/
    /// </summary>
    public static string FtpDirectory(string rootDirectory)
    {
        rootDirectory = rootDirectory.Trim('/');
        return string.Format(@"/{0}/", rootDirectory);
    }

    /// <summary>
    /// string p1 = "/temp/";
    /// string p2 = "/subdir/file/";
    /// to /temp/subdir/file/
    /// </summary>
    public static string CombineDirectory(string rootDirectory, string childDirectory)
    {
        rootDirectory = rootDirectory.Trim('/');
        childDirectory = childDirectory.Trim('/');
        return string.Format(@"/{0}/{1}/", rootDirectory, childDirectory);
    }

    /// <summary>
    /// string p1 = "/temp/";
    /// string p2 = "file.text";
    /// to /temp/file.text
    /// </summary>
    public static string CombineFile(string rootDirectory, string filePathOrName)
    {
        rootDirectory = rootDirectory.Trim('/'); ;
        filePathOrName = filePathOrName.Trim('/'); ;
        return string.Format(@"/{0}/{1}", rootDirectory, filePathOrName);
    }

    public static string ServerDetails
        (string host, string port, string userName, string type = "FTP")
    {
        return String.Format("Type: '{3}' Host:'{0}' Port:'{1}' User: '{2}'", host, port, userName, type);
    }
}
