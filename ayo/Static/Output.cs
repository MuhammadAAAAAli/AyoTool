using System;
using System.Diagnostics;
using System.IO;
using ayo.Interfaces;

namespace ayo.Static
{
    public class Output : IOutput
    {
        private readonly object SyncObject = new object();
        private StreamWriter PassGenerated;

        public Output(string outputPasswordFile)
        {
            InitializePasswordOutputFile(outputPasswordFile);
            Watch.Start();
        }

        public void LogGeneratedPasswords(string password)
        {
            lock (SyncObject)
            {
                PassGenerated.WriteLine(password);
                PassGenerated.Flush();
            }
        }

        private void InitializePasswordOutputFile(string outputPasswordFile)
        {
            PassGenerated = new StreamWriter(outputPasswordFile, false);
        }

        #region Static variables

        private static readonly Stopwatch Watch = new Stopwatch();
        private static readonly object SyncObjectS = new object();
        private static readonly StreamWriter LogFile = new StreamWriter("log.txt", true);

        private static readonly StreamWriter DebugJson = new StreamWriter("debugJson.txt", false);
            // file just for tests

        #endregion

        #region Static output functions

        public static void Log(string logMessage)
        {
            lock (SyncObjectS)
            {
                LogFile.Write("\n{0} {1}  : ", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                LogFile.Write(logMessage);
                LogFile.Flush();
            }
        }

        public static void WriteJsonForDebug(string logMessage)
        {
            lock (SyncObjectS)
            {
                DebugJson.WriteLine(logMessage);
                DebugJson.Flush();
            }
        }

        public static string LogTime()
        {
            int elapsedSeconds;
            lock (SyncObjectS)
            {
                Watch.Stop();
                elapsedSeconds = (int) (Watch.ElapsedMilliseconds/1000);
                Watch.Start();
            }
            return (elapsedSeconds/60 + " minutes " + elapsedSeconds%60 + " seconds !");
        }

        public static void ToConsole(string logMessage)
        {
            lock (SyncObjectS)
            {
                Console.WriteLine(logMessage);
            }
        }

        #endregion
    }
}