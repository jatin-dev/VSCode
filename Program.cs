using System;

namespace VSCode
{
    class Program
    {
        static void Main(string[] args)
        {
            // Start the server  
            //TcpHelper.StartServer(5678);  
            //TcpHelper.Listen(); // Start listening.
            string value = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
            Console.WriteLine("hello"+value);

            AsynchronousSocketListener.StartListening(); 

           //AsynchronousClient.StartClient();
            return;
        }
    }
}
