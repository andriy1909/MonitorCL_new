using MonitorLibrary.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MonitorLibrary
{
    public class ServerObject : IDisposable
    {
        public static TcpListenerEx tcpListener;// сервер для прослушивания
        public string IP { get; private set; }
        public int Port { get; private set; }
        public static List<ClientObject> clients = new List<ClientObject>();// все подключения
        private Thread listenerThread;
        MonitoringDB db = new MonitoringDB();

        public delegate void ReceiveDelegate(string message);

        public ReceiveDelegate receiveOut;

        public void setReceiveOut(ReceiveDelegate receive)
        {
            receiveOut = receive;
        }

        public void StartServer(string ip, int port)
        {
            MonitoringDB.ReCreareDB();

            IP = ip;
            Port = port;
            listenerThread = new Thread(Listen);
            listenerThread.Start();
        }

        public void Listen()
        {
            try
            {
                tcpListener = new TcpListenerEx(IPAddress.Parse(IP), Port);
                tcpListener.Start();
                Debug.WriteLine("Сервер запущен.");
                receiveOut("Сервер запущен.");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    ClientObject clientObject = new ClientObject(tcpClient, this);
                    clientObject.setReceiveOut(receiveOut);
                    AddConnection(clientObject);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Dispose();
            }
        }

        public void RemoveConnection(int Id)
        {
            //получаем по id закрытое подключение
            ClientObject client = clients.FirstOrDefault(c => c.Id == Id);
            //и удаляем его из списка подключений
            if (client != null)
                clients.Remove(client);
        }
        
        public void AddConnection(ClientObject clientObject)
        {
            Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
            clientThread.Start();
        }

        // отключение всех клиентов
        public void Dispose()
        {
            //***остановка сервера только после закрития всех подключений**

            tcpListener.Stop(); //остановка сервера

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Dispose(); //отключение клиента
            }
            Environment.Exit(0); //завершение процесса
        }
    }
}
