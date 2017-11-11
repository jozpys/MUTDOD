using System;
using System.IO;
using MUTDOD.Common;
using MUTDOD.Common.Settings;

namespace MUTDOD.Server.Common.Tools.Logger
{
    /// <summary>
    /// A basic temporary logger.
    /// </summary>
    public class Logger : ILogger
    {
        private readonly object SyncRoot = new object();

        private readonly object SyncWrite = new object();

        private TextWriter _logStream;

        private LogLevel _logLevel;

        public string Name
        {
            get { return Constant.Name; }
        }

        public LogLevel LogLevel
        {
            get
            {
                return _logLevel;
            }

            set
            {
                lock (SyncRoot)
                {
                    Log( 
                                Name, 
                                String.Format(LogMessage.LoggerLevelChangingFromFormat, _logLevel.ToString()),
                                MessageLevel.Info);
                    _logLevel = value;
                    Log(Name, String.Format(LogMessage.LoggerLevelChangedToFormat, value.ToString()), MessageLevel.Info);
                }
            }
        }

        public void Close()
        {
            if (_logStream != null)
            {
                Log(Name, LogMessage.LoggingStopped, MessageLevel.Info);

                _logStream.Close();
                _logStream = null;
            }
        }

        public Logger(ISettingsManager settingsManager, LogLevel requestLogLevel)
        {
            lock (SyncRoot)
            {
                _logLevel = requestLogLevel;

                var path = settingsManager.LogFileDirectory;
                DateTime dateTime = DateTime.Now;

                if (!Directory.Exists(path))
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                if (Directory.Exists(path))
                {
                    string fileName = String.Format(
                                                    Constant.LogFilePathAndNameFormat, 
                                                    path, 
                                                    dateTime.ToString(Constant.LogFileNameDateFormat));
                    _logStream = new StreamWriter(fileName);
                }

                Log(Name, LogMessage.LoggingStarted, MessageLevel.Info);
            }
        }

        public void Log(string senderName, string message, MessageLevel messageLevel)
        {
            
                if (((uint)_logLevel & (uint)messageLevel) != 0)
                {
                    string logMessageHeader = String.Format(
                                                        LogMessage.LogHeaderFormat, 
                                                        DateTime.Now.ToString(Constant.LogDateTimeFormat),
                                                        messageLevel.ToString());

                    string logMessageBody = String.Format(LogMessage.LogBodyFormat, senderName, message);

                    lock (SyncWrite)
                    {
                        if (_logStream != null)
                        {
                            _logStream.WriteLine(logMessageHeader);
                            _logStream.WriteLine(logMessageBody);
                            _logStream.WriteLine();
                        }
                    }
                }
        }

        private static class Constant
        {
            public const string Name = "Logger";
            public const string LogFilePathAndNameFormat = "{0}MUTLogger.LOGLogger.{1}.txt";
            public const string LogFileNameDateFormat = "ddMMyyyyLogger.HHmmss[fffffff]";
            public const string LogDateTimeFormat = "dd/MM/yyyy HH:mm:ss:fffffff";
        }

        private static class LogMessage
        {
            public const string LoggerLevelChangingFromFormat = "Logger level changing from {0}";
            public const string LoggerLevelChangedToFormat = "Logger level changed to {0}";
            public const string LoggingStarted = "Logging has been started.";
            public const string LoggingStopped = "Logging has been stopped.";
            public const string LogHeaderFormat = "{0}   [{1}]";
            public const string LogBodyFormat = "<<{0}>>   {1}";
        }
    }
}
