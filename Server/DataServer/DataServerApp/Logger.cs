using System;
using MUTDOD.Common;
using MUTDOD.Common.Settings;
using MUTDOD.Server.Common.Tools.Logger;

namespace MTDOD.DataServerApp
{
    internal class Logger : ILogger
    {
        private readonly Action<LogItem> _guiLogAction;
        private ILogger _logger;
        public Logger(ISettingsManager settingsManager, Action<LogItem> guiLogAction)
        {
            _guiLogAction = guiLogAction;
            _logger = new MUTDOD.Server.Common.Tools.Logger.Logger(settingsManager, LogLevel.All);
        }

        public void Log(string senderName, string message, MessageLevel messageLevel)
        {
            if (_guiLogAction != null)
                _guiLogAction(new LogItem(senderName, messageLevel.ToString(), message));
            try
            {
                _logger.Log(senderName, message, messageLevel);
            }
            catch (Exception ex)
            {
                if (_guiLogAction != null)
                    _guiLogAction(new LogItem("Logger", MessageLevel.Error.ToString(), ex.ToString()));
                else
                    throw ex;
            }
        }

        public struct LogItem
        {
            public string SenderName;
            public string Message;
            public string MessageLevel;

            public LogItem(string senderName, string messageLevel, string message)
                : this()
            {
                SenderName = senderName;
                MessageLevel = messageLevel;
                Message = message;
            }
        }
    }
}
