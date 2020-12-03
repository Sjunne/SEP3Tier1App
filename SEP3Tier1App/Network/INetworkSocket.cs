using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Data;

namespace SEP3Tier1App.Network
{
    public interface INetworkSocket
    {
        Task getConnections(string username);
        Delegating getDelegating();
        void setUsername(string username);
        void SendUsername();
        void SendMessage(PrivateMessage message);
        void closeConnection();
    }
}