using LKZ.Server.Managers;
using LKZ.Server.Network.Objects;
using System.Net.Sockets;

public class BaseClient
{
    private uint id;
    private TcpClient tcpClient;
    private uint playerId;
    private Lobby lobby;
    private int ping = 100; //ms
    public BaseClient(uint id, TcpClient tcpClient)
    {
        Id = id;
        TcpClient = tcpClient;
    }

    public int Ping
    {
        get { return ping; }
        set { ping = value; }
    }
    public uint Id
    {
        get { return id; }
        private set { id = value; }
    }

    public TcpClient TcpClient
    {
        get { return tcpClient; }
        private set { tcpClient = value; }
    }

    public uint PlayerId
    {
        get { return playerId; }
        set { playerId = value; }
    }

    public Lobby Lobby
    {
        get { return lobby; }
        set { lobby = value; }
    }
}
