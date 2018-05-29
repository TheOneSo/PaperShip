using System;
using System.Text;

using UIKit;

namespace PaperShip
{
    public enum Command
    {
        SIGN_IN, BIND, SIGN_OUT, SEND_MSG, LOGIN_EXISTS,
        FIND_USER, ONLINE, TEST, QUIT, UNKNOWN, ERROR
    }

    public class CommandHandler
    {
        //Используется для сервера ( не обращать внимания )
        const string _space = "\u0000";
        const string _end = "\u0001";
        string end = string.Format("{0}END{1}", _end, _space);

        StringBuilder builder;

        public string AddCommandAndSeparator(Command command, params string[] args)
        {
            builder = new StringBuilder(command.ToString());
            foreach (var item in args)
                builder.Append(_space).Append(item);
            builder.Append(end);

            return builder.ToString();
        }

        public void SplitOnEndMessage(string msg)
        {
            string[] messages = msg.Split(new string[] { end }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in messages)
                Handler(item);
        }

        Command GetCommand(string command)
        {
            foreach(var item in Enum.GetValues(typeof(Command)))
            {
                if (item.ToString().Equals(command.ToUpper()))
                    return (Command)item;
            }
            return Command.UNKNOWN;
        }

        void Handler(string str)
        {
            string[] texts = str.Split(new string[] { _space }, StringSplitOptions.RemoveEmptyEntries);

            //Command command = GetCommand(texts[0]);

            foreach(var item in texts)
                Console.Write(item + " ");
        }
    }
}
