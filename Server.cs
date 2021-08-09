using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TirntirobSharp
{
    class Server
    {
        public string IP = "0.0.0.0";
        public int Port = 80;

        private Socket server;
        private Thread thread;
        private bool isRunning = false;
        private bool isStopped = false;

        public Exception Start()
        {
            var ipAddress = IPAddress.Parse(IP);
            var endPoint = new IPEndPoint(ipAddress, Port);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Bind(endPoint);
                Console.WriteLine("Listening " + IP + ":" + Port + "...");
                isRunning = true;
                isStopped = false;
                thread = new Thread(() =>
                {
                    Thread lt = new Thread(() =>
                    {
                        while (isRunning)
                        {
                            server.Listen(0);
                            Accept(server.Accept());
                        }
                        isStopped = true;
                    })
                    { IsBackground = true };
                    lt.Start();
                    while (isRunning) Thread.Sleep(100);
                    if(!isStopped) lt.Interrupt();
                    server.Close();
                    isStopped = true;
                })
                { IsBackground = false };
                thread.Start();
                return null;
            }
            catch (Exception e)
            {
                return e;
            }
        }

        public void Stop()
        {
            isRunning = false;
            while (!isStopped) Thread.Sleep(0);
        }

        private void Accept(Socket client)
        {
            new Thread(() =>
            {
                string rip = ((IPEndPoint)client.RemoteEndPoint).Address.ToString();
                if (BlockList.IsBlocked(rip))
                {
                    client.Close();
                    return;
                }
                int timeout = 5000;
                int ms = DateTime.Now.Millisecond;
                Thread t = new Thread(() =>
                {
                    byte[] bytes = new byte[1024];
                    string request = "";
                    bool r = true;
                    while (r)
                    {
                        client.Receive(bytes);
                        request += Encoding.UTF8.GetString(bytes);
                        r = request.Contains("\n\n");
                    }
                    ms = DateTime.Now.Millisecond;
                    if (Str.SubStrCount(request, ' ') > 1 && Str.SubStrCount(request, '\n') > 1)
                    {
                        var rqst = new Request
                        {
                            RemoteIP = rip,
                            LocalIP = ((IPEndPoint)client.LocalEndPoint).Address.ToString(),
                            Method = request.Substring(0, request.IndexOf(' '))
                        };
                        request = request[(request.IndexOf(' ') + 1)..];
                        rqst.URI = request.Substring(0, request.IndexOf(' '));
                        request = request[(request.IndexOf(' ') + 1)..];
                        rqst.HTTPv = request.Substring(0, request.IndexOf('\n'));
                        request = request[(request.IndexOf('\n') + 1)..];
                        foreach (string header in request.Split('\n')) if (header.Contains(':')) rqst.Headers.Set(header.Substring(0, header.IndexOf(':')), header[(header.IndexOf(':') + 1)..]);
                        var response = PageManager.GetResponse(rqst);
                        if (!response.ForceClose)
                        {
                            if (response.HTTPResponse) client.Send(Encoding.UTF8.GetBytes(response.HTTPv+' '+response.Code+CodeMessages.Get(response.Code)+"\nContent-Length: "+response.Result.Length+"\nContent-type: "+response.ContentType+"\nConnection: Closed\n\n"));
                            client.Send(response.Result);
                        }
                    }
                    client.Close();
                })
                {
                    IsBackground = true
                };
                t.Start();
                while (t.IsAlive)
                {
                    if (DateTime.Now.Millisecond - ms > timeout) {
                        t.Interrupt();
                        client.Close();
                    }
                    else Thread.Sleep(1000);
                }
                GC.Collect();
            })
            {
                IsBackground = true
            }.Start();
        }
    }
}
