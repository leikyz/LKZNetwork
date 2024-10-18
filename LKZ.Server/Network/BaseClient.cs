using LKZ.Server.Managers;
using System.Net.Sockets;

public class BaseClient
{
    public uint Id { get; set; }
    public TcpClient TcpClient { get; set; }

    public uint PlayerId { get; set; }

    public Lobby Lobby { get; set; }

    public BaseClient(uint id, TcpClient tcpClient)
    {
        Id = id;
        TcpClient = tcpClient;
    }
}
