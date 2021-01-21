using FluentFTP;
using System.IO;
using System.Threading.Tasks;

public abstract class FtpContext : IRemoteFileSystemContext
{
    protected IFtpClient FtpClient { get; set; }

    public void Connect()
    {
        FtpClient.Connect();
    }

    public void Disconnect()
    {
        FtpClient.Disconnect();
    }

    public void Dispose()
    {
        if (FtpClient != null && !FtpClient.IsDisposed)
        {
            FtpClient.Dispose();
        }
    }

    /*actions*/
    public bool FileExists(string filePath)
    {
        return FtpClient.FileExists(filePath);
    }

    public void DeleteFileIfExists(string filePath)
    {
        if (!FileExists(filePath))
        {
            FtpClient.DeleteFile(filePath);
        }
    }

    public void UploadFile(string localFilePath, string remoteFilePath)
    {
        FtpClient.UploadFile(localFilePath, remoteFilePath);
    }

    public bool DirectoryExists(string directoryPath)
    {
        return FtpClient.DirectoryExists(directoryPath);
    }

    public void CreateDirectoryIfNotExists(string directoryPath)
    {
        if (!DirectoryExists(directoryPath))
        {
            FtpClient.CreateDirectory(directoryPath);
        }
    }

    public void DownloadFile(string localFilePath, string remoteFilePath)
    {
        FtpClient.DownloadFile(localFilePath, remoteFilePath);
    }

    public async Task<Stream> ReadAsync(string path)
    {
        return await FtpClient.OpenReadAsync(path);
    }

    public bool IsConnected()
    {
        return FtpClient.IsConnected;
    }

    public void SetWorkingDirectory(string directoryPath)
    {
        FtpClient.SetWorkingDirectory(directoryPath);
    }

    public void SetRootAsWorkingDirectory()
    {
        SetWorkingDirectory("");
    }

    public abstract string ServerDetails();
    public abstract void SetHost(string host);
}