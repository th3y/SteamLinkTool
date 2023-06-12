using System;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SteamLauncher
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static class SteamLinkArgs
        {
            public static string epicUrl;
            public static string exeName;
            public static int timeout;
        }

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, 0); // 0 hide - 5 show

            string launcherName = "EpicGamesLauncher";
            if (args.Length < 2)
            {
                System.Console.WriteLine("ERROR: Needs launch URL and EXE Name");
                return;
            }
            SteamLinkArgs.epicUrl = args[0];
            SteamLinkArgs.exeName = args[1];

            if (args.Length == 3)
                SteamLinkArgs.timeout = int.Parse(args[2]);

            Ini iniFile = new Ini("config.ini");

            string inival = iniFile.GetValue("RestartEpicGames").ToLower();

            if (inival != "false") {
                foreach (var process in Process.GetProcessesByName(launcherName))
                {
                    process.Kill();
                }
            }

            // Execute Epic Games Application
            var ps = new ProcessStartInfo( SteamLinkArgs.epicUrl )
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(ps);

            System.Console.WriteLine($"Starting : { SteamLinkArgs.exeName }");
            
            // TimeOut, because epic games launcher is too slow to run the game
            inival = iniFile.GetValue("TimeOutSeconds");

            if (inival.Length <= 1) inival = "60";
            int waitTime = int.Parse(inival);

            if (SteamLinkArgs.timeout >= 5) waitTime = SteamLinkArgs.timeout;

            Thread.Sleep(waitTime * 1000);

            var gameProcesses = Process.GetProcessesByName( SteamLinkArgs.exeName );

            if (gameProcesses.Length != 1)
            {
                System.Console.WriteLine($"Could not find a single process with name: { SteamLinkArgs.exeName }");
                return;
            }
            
            System.Console.WriteLine($"Game started.");
            gameProcesses[0].WaitForExit();

            Thread.Sleep(2000);

            foreach (var process in Process.GetProcessesByName(launcherName))
                process.Kill();

            Environment.Exit(Environment.ExitCode);
        }
    }
}