using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MonitorLibrary
{
    public class ClientObject : IDisposable
    {
        public int Id { get; set; }
        public NetworkStream Stream { get; private set; }
        public TcpClient client;
        private ServerObject server; // объект сервера     
        private Thread receiveThread;
        public delegate void ReceiveDelegate(string message);
        public ReceiveDelegate receiveOut;

        public void setReceiveOut(ReceiveDelegate receive)
        {
            receiveOut = receive;
        }



        public ClientObject()
        {

        }
        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            client = tcpClient;
            server = serverObject;
            Stream = client.GetStream();
        }
        public void Connect(string ip, int port)
        {
            if (client == null)
                client = new TcpClient();

            try
            {
                client.Connect(ip, port);
                Stream = client.GetStream();
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
            }
        }
        public void Process()
        {
            try
            {
                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        string message = GetMessage();
                        Debug.WriteLine("message: " + message);

                        server.receiveOut(message);
                    }
                    catch (Exception err)
                    {
                        Debug.WriteLine("Error process : {0}", err.Message);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.RemoveConnection(this.Id);
                Debug.WriteLine("выход из Process()");
                Dispose();
            }
        }

        // чтение входящего сообщения и преобразование в строку
        public string GetMessage()
        {
            byte[] data = new byte[256]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                if (builder.ToString() == "")
                {
                    continue;
                }
            }
            while (Stream.DataAvailable);
            
            return builder.ToString();
        }

        /// <summary>
        /// закрытие подключения
        /// </summary>        
        public void Dispose()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}
