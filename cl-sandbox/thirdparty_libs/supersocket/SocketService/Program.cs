using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Reflection;
using SuperSocket.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;
using SuperSocket.SocketEngine.Configuration;


namespace SuperSocket.SocketService
{
    static partial class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {            
            if ((!Platform.IsMono && !Environment.UserInteractive)//Windows Service
                || (Platform.IsMono && !AppDomain.CurrentDomain.FriendlyName.Equals(Path.GetFileName(Assembly.GetEntryAssembly().CodeBase))))//MonoService
            {
                RunAsService();
                return;
            }

            if (args != null && args.Length > 0)
            {
                if (args[0].Equals("-i", StringComparison.OrdinalIgnoreCase))
                {
                    SelfInstaller.InstallMe();
                    return;
                }
                else if (args[0].Equals("-u", StringComparison.OrdinalIgnoreCase))
                {
                    SelfInstaller.UninstallMe();
                    return;
                }

                Console.WriteLine("Invalid argument!");
                return;
            }
            
            RunAsConsole();
        }

        private static bool setConsoleColor;

        static void CheckCanSetConsoleColor()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.ResetColor();
                setConsoleColor = true;
            }
            catch
            {
                setConsoleColor = false;
            }
        }

        private static void SetConsoleColor(ConsoleColor color)
        {
            if (setConsoleColor)
                Console.ForegroundColor = color;
        }

        static void RunAsConsole()
        {
            CheckCanSetConsoleColor();

            Console.WriteLine("Press any key to start the SuperSocket ServiceEngine!");
            Console.ReadKey();
            Console.WriteLine();

            Console.WriteLine("Initializing...");

            IBootstrap bootstrap = BootstrapFactory.CreateBootstrap();

            if (!bootstrap.Initialize())
            {
                SetConsoleColor(ConsoleColor.Red);

                Console.WriteLine("Failed to initialize SuperSocket ServiceEngine! Please check error log for more information!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Starting...");

            var result = bootstrap.Start();

            Console.WriteLine("-------------------------------------------------------------------");

            foreach (var server in bootstrap.AppServers)
            {
                if (server.State == ServerState.Running)
                {
                    SetConsoleColor(ConsoleColor.Green);
                    Console.WriteLine("- {0} has been started", server.Name);
                }
                else
                {
                    SetConsoleColor(ConsoleColor.Red);
                    Console.WriteLine("- {0} failed to start", server.Name);
                }
            }

            Console.ResetColor();
            Console.WriteLine("-------------------------------------------------------------------");

            switch(result)
            {
                case(StartResult.None):
                    SetConsoleColor(ConsoleColor.Red);
                    Console.WriteLine("No server is configured, please check you configuration!");
                    Console.ReadKey();
                    return;

                case(StartResult.Success):
                    Console.WriteLine("The SuperSocket ServiceEngine has been started!");
                    break;

                case (StartResult.Failed):
                    SetConsoleColor(ConsoleColor.Red);
                    Console.WriteLine("Failed to start the SuperSocket ServiceEngine! Please check error log for more information!");
                    Console.ReadKey();
                    return;

                case (StartResult.PartialSuccess):
                    SetConsoleColor(ConsoleColor.Red);
                    Console.WriteLine("Some server instances were started successfully, but the others failed! Please check error log for more information!");
                    break;
            }

            Console.ResetColor();
            Console.WriteLine("Press key 'q' to stop the ServiceEngine.");

            while (Console.ReadKey().Key != ConsoleKey.Q)
            {
                Console.WriteLine();
                continue;
            }

            bootstrap.Stop();

            Console.WriteLine();
            Console.WriteLine("The SuperSocket ServiceEngine has been stopped!");
        }

        static void RunAsService()
        {
            ServiceBase[] servicesToRun;

            servicesToRun = new ServiceBase[] { new MainService() };

            ServiceBase.Run(servicesToRun);
        }
    }
}