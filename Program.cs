using System;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Net.NetworkInformation;
using System.Linq;

namespace SteamLauncher
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static class ExeLinkArgs
        {
            public static string url;
            public static string exeName;
            public static int timeout;
        }
        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, 0); // 0 hide - 5 show

            string appName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
            string launcherName = "EpicGamesLauncher";

            Ini iniFile = new Ini("config.ini");

            string filepath = iniFile.GetValue("GamePath").ToLower();
            string timeoutstr = iniFile.GetValue("TimeOutSeconds").Trim();

            bool bUsingPathIni = false;
            
            if (args.Length == 1)
            {
                ExeLinkArgs.exeName = args[0];
            }
            else if (args.Length == 2)
            {
                ExeLinkArgs.url = args[0];
                ExeLinkArgs.exeName = args[1];
            }
            else if (filepath.Length != 0)
            {
                bUsingPathIni = true;
                ExeLinkArgs.url = filepath;
                ExeLinkArgs.exeName = Path.GetFileNameWithoutExtension(filepath);

                // In case im running an epic games launcher + Remote Play
                if (filepath.Contains("com.epicgames.launcher:"))
                {
                    Console.WriteLine($"Running from epic {filepath}");
                    string[] array = filepath.Split(' ');
                    ExeLinkArgs.url = array[0];
                    ExeLinkArgs.exeName = array[1];
                    bUsingPathIni = false;
                }
            }
            else
            {
                Console.WriteLine("ERROR: Needs launch URL and EXE Name");
                return;
            }

            var currentProcess = Process.GetCurrentProcess();
            var pcss = Process.GetProcessesByName(currentProcess.ProcessName);
            var anotherProcess = Process.GetProcesses().FirstOrDefault(p => p.ProcessName == currentProcess.ProcessName && p.Id != currentProcess.Id);

            if (pcss.Length >= 3) return;

            // Slave Work
            if (pcss.Length == 2 && anotherProcess != null && !bUsingPathIni)
            {
                Console.WriteLine($"Slave running and looking for my mastah");

                while (true)
                {
                    var mainprocess = Process.GetProcesses().FirstOrDefault(p => p.ProcessName == currentProcess.ProcessName && p.Id != currentProcess.Id);
                    if (mainprocess == null)
                    {
                        // Kill the game called from EpicGames
                        foreach (var process in Process.GetProcessesByName(ExeLinkArgs.exeName))
                            process.Kill();

                        // Kill EpicGamesLauncher
                        foreach (var process in Process.GetProcessesByName(launcherName))
                            process.Kill();

                        Environment.Exit(Environment.ExitCode);
                        break;
                    }

                    Thread.Sleep(1000);
                }

                return;
            }

            if (args.Length == 3)
                ExeLinkArgs.timeout = int.Parse(args[2]) * 1000;
            else
                ExeLinkArgs.timeout = (timeoutstr.Length != 0) ? int.Parse(timeoutstr) * 1000 : (70 * 1000);

            Stopwatch detector = Stopwatch.StartNew();

            if (bUsingPathIni)
            {

                // Execute Application
                var ps = new ProcessStartInfo(ExeLinkArgs.url)
                {
                    UseShellExecute = true,
                    WorkingDirectory = Path.GetDirectoryName(ExeLinkArgs.url),
                    Verb = "open"
                };
                Process game = Process.Start(ps);

                Console.WriteLine($"Starting from path: {ExeLinkArgs.exeName}");
                Console.WriteLine($"Timeout: {ExeLinkArgs.timeout / 1000} sec");

                bool gameIsRunning = false;

                while (detector.ElapsedMilliseconds < ExeLinkArgs.timeout && !gameIsRunning)
                {
                    Process[] processes = Process.GetProcessesByName(ExeLinkArgs.exeName);
                    foreach (Process proc in processes)
                    {
                        if (proc.Id == game.Id) // Check if the PID matches the process we started
                        {
                            Console.WriteLine($"Process {ExeLinkArgs.exeName} running, waiting until terminated.");

                            detector.Stop();
                            proc.WaitForExit();
                            detector.Restart();
                            gameIsRunning = true;
                            break;
                        }
                    }

                    Thread.Sleep(1000);
                }

                //if (detector.ElapsedMilliseconds >= SteamLinkArgs.timeout)
                //{
                //    Console.WriteLine($"Timeout ({SteamLinkArgs.timeout / 1000} seconds) reached. Exiting.");
                //}
                //else
                //{
                //    Console.WriteLine($"Process {SteamLinkArgs.exeName} exited, closing.");
                //}

                Thread.Sleep(500);

                Environment.Exit(Environment.ExitCode);
            }
            else
            {
                // Kill Epic Games Launcher in case
                string inival = iniFile.GetValue("RestartEpicGames").ToLower();

                if (inival != "false")
                {
                    foreach (var process in Process.GetProcessesByName(launcherName))
                    {
                        process.Kill();
                    }
                }

                // Execute Epic Games Launcher + Game parameter
                var ps = new ProcessStartInfo(ExeLinkArgs.url)
                {
                    UseShellExecute = true,
                    Verb = "open"
                };
                Process.Start(ps);

                Console.WriteLine($"Starting: {ExeLinkArgs.exeName}");
                Console.WriteLine($"Timeout: {ExeLinkArgs.timeout / 1000} sec");

                if (pcss.Length == 1) // start slave
                {
                    string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    Console.WriteLine($"Running slave");
                    string strCmdPars = $"/C start /min \"\" /D \"{appDirectory}\" {appName} \"{ExeLinkArgs.exeName}\"";
                    Process.Start("CMD.exe", strCmdPars);
                }

                while (detector.ElapsedMilliseconds < ExeLinkArgs.timeout)
                {
                    var gameProcess = Process.GetProcessesByName(ExeLinkArgs.exeName);
                    if (gameProcess.Length > 0)
                    {
                        Console.WriteLine($"Process {ExeLinkArgs.exeName} running, waiting until terminated");

                        detector.Stop();
                        gameProcess[0].WaitForExit();
                        detector.Restart();
                        break;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }

               
                //if (detector.ElapsedMilliseconds >= SteamLinkArgs.timeout)
                //{
                //    Console.WriteLine($"Timeout ({SteamLinkArgs.timeout / 1000} seconds) reached. Exiting.");
                //}
                //else
                //{
                //    System.Console.WriteLine($"Process {SteamLinkArgs.exeName} exited, closing.");
                //}

                Thread.Sleep(500);

                // Kill Epic Games Launcher
                foreach (var process in Process.GetProcessesByName(launcherName))
                    process.Kill();

                Environment.Exit(Environment.ExitCode);

            }
        }
    }
}