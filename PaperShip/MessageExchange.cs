using System;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PaperShip
{
    public class MessageExchange
    {
        Server server;
        CommandHandler command;
        StringBuilder builder;

        public MessageExchange()
        {
            server = Server.GetServer();
            command = new CommandHandler();
        }

        void SendMessage(string text)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(text);
                server.Network.Write(data, 0, data.Length);
            } catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [Obsolete("Устаревший вариант! Будет новый")]
        public void SendRegistration(string number, string device)
        {
            string msg = command.AddCommandAndSeparator(Command.BIND, number, device);
            SendMessage(msg);
        }

        public async Task Registration(string number, string device)
        {
            string reg = command.AddCommandAndSeparator(Command.BIND, number, device);
            string reply = await RecievedMessageAsync();
        }

        // Запускается в потоке ( постоянное чтение )
        public void RecievedMessage()
        {
            while(true)
            {
                try
                {
                    builder = new StringBuilder();
                    byte[] data = new byte[1024];
                    do
                    {
                        int bytes = server.Network.Read(data, 0, data.Length);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    } while (server.Network.DataAvailable);

                    command.SplitOnEndMessage(builder.ToString());
                } catch
                {
                    server.Disconnected();
                    throw new Exception("Временная ошибка RecievedMessage");
                }
            }
        }

        // Асинхронная отправка данных
        protected async Task SendMessageAsync(string text)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(text);
                await server.Network.WriteAsync(data, 0, data.Length);
            } catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Асинхронное чтение данных ( возвращает string )
        protected async Task<string> RecievedMessageAsync()
        {
            try
            {
                builder = new StringBuilder();
                byte[] data = new byte[1024];

                int bytes = await server.Network.ReadAsync(data, 0, data.Length);
                builder.Append(Encoding.UTF8.GetString(data, 0, bytes));

                return builder.ToString();
            } catch
            {
                throw new Exception("Временная ошибка! RecievedMessage Async");
            }
        }
    }
}
