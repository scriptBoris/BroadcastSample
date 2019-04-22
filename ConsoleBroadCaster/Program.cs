using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBroadCaster
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Program in running");

            int PORT = 8008;
            var udpClient = new UdpClient();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, PORT));

            var from = new IPEndPoint(0, 0);
            while (true)
            {
                var recvBuffer = udpClient.Receive(ref from);

                string str4 = "Eah, im server!";
                byte[] sendBytes4 = Encoding.ASCII.GetBytes(str4);
                udpClient.Send(sendBytes4, sendBytes4.Length, from);
                Console.WriteLine(Encoding.UTF8.GetString(recvBuffer));
            }
        }
    }
}
