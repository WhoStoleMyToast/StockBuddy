using FluentFTP;

public class FtpRemoteFileSystem : FtpContext
{
    private string _serverDetails;

    public FtpRemoteFileSystem(RemoteSystemSetting setting)
    {
        _serverDetails = FtpHelper.ServerDetails
             (setting.Host, setting.Port.ToString(), setting.UserName, setting.Type);
        FtpClient = new FtpClient(setting.Host);
        FtpClient.Credentials = null;
        //FtpClient.Credentials = new System.Net.NetworkCredential(setting.UserName, setting.Password);
        FtpClient.Port = setting.Port;
    }

    public override void SetHost(string host)
    {
        FtpClient.Host = host;
    }

    public override string ServerDetails()
    {
        return _serverDetails;
    }
}