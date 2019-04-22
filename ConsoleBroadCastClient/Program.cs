using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBroadCastClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Client is running");


            while (true)
            {
                Console.ReadLine();

                int GroupPort = 8008;
                UdpClient udp = new UdpClient();

                //IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse("255.255.255.255"), GroupPort);
                IPEndPoint groupEP = new IPEndPoint(IPAddress.Broadcast, GroupPort);
                string str4 = "Is anyone out there?";
                byte[] sendBytes4 = Encoding.ASCII.GetBytes(str4);

                udp.Send(sendBytes4, sendBytes4.Length, groupEP);

                byte[] receiveBytes = udp.Receive(ref groupEP);

                string returnData = Encoding.ASCII.GetString(receiveBytes);

                //string returnData = Encoding.ASCII.GetString(receiveBytes, 0, receiveBytes.Length);

                //udp.Close();

                Console.WriteLine("Response: " + returnData);
            }
        }
    }
}
