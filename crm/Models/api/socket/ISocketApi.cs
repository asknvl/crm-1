using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.api.socket
{
    public interface ISocketApi
    {
        Task Connect(string token);
        void RequestConnectedUsers();

        public event Action<List<string>> ReceivedConnectedUsersEvent;
    }

    public class SocketApiException : Exception
    {
        public SocketApiException(string msg) : base(msg) { }
    }
}
