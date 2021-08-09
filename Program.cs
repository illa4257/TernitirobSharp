using System;

namespace TirntirobSharp
{
    class Program
    {
        static void Main(string[] _1)
        {
            var p = new Program();
            p.Exec();
        }

        public void Exec()
        {
            var serv = new Server();
            var e = serv.Start();
            if (e != null)
            {
                Console.WriteLine("Error: " + e.Message);
                return;
            }
            Console.WriteLine("Press any key, to stop the server...");
            Console.ReadKey();
            Console.WriteLine("Stopping...");
            serv.Stop();
            Console.WriteLine("Stopped!");
        }
    }
}
