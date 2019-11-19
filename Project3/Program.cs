//@author: Isaias Villalobos
//@date: 11/19/2019
//@description: This class will handle input the messenger application
//              This class will use another class, to handle the speicific command the user entered.
//              This class will output any useful messenges to the user.


using System;

namespace Project3
{
    class Program
    {
       
        public static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                Console.WriteLine("dotnet run <option> <other arguments>");
                return;
            }
            if (args[0] == "sendMsg")
            {
                if (args.Length < 3)
                {
                    Console.WriteLine("dotnet run <option> <email> <plaintext> ");
                    return;
                }
                var URL = args[1];
                var plainText = args[2];
                ProjectFunctions functions = new ProjectFunctions();
                functions.sendMsg(URL, plainText);
                Console.WriteLine("Message written");
                Console.ReadLine();
            }
            if (args[0] == "sendKey")
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("dotnet run <option> <email>");
                    return;
                }
                var email = args[1];

                ProjectFunctions functions = new ProjectFunctions();
                functions.sendKey(email);
                Console.WriteLine("Key saved");
                Console.ReadLine();
            }
            if (args[0] == "getMsg")
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("dotnet run <option> <email>");
                    return;
                }

                var email = args[1];
                ProjectFunctions functions = new ProjectFunctions();
                functions.getMsg(email);
                Console.WriteLine("Message written");
                Console.ReadLine();

            }
            if (args[0] == "getKey")
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("dotnet run <option> <email>");
                    return;
                }
                ProjectFunctions functions = new ProjectFunctions();
                functions.getkey(args[1]);
                Console.ReadLine(); 
            }
            if (args[0] == "keygen")
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("dotnet run <option> <bitsize>");
                    return;
                }
                ProjectFunctions functions = new ProjectFunctions();
                functions.keyGen(Convert.ToInt32(args[1]));
                Console.ReadLine(); 
            }
        }
    }
}
