public class RemoteSystemSetting
{
    public string Host { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Type { get; } = "FTP";
    public int Port { get; set; }
}
