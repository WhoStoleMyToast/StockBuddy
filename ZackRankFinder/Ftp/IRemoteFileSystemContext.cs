using System;
using System.IO;
using System.Threading.Tasks;

public interface IRemoteFileSystemContext : IFileSystem, IDisposable
{
    bool IsConnected();
    void Connect();
    void Disconnect();

    void SetWorkingDirectory(string path);
    void SetRootAsWorkingDirectory();

    void UploadFile(string localFilePath, string remoteFilePath);
    void DownloadFile(string localFilePath, string remoteFilePath);
    Task<Stream> ReadAsync(string path);

    string ServerDetails();
    void SetHost(string host);
}
