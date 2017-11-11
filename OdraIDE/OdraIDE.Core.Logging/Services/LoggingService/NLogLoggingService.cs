using System;
using System.ComponentModel.Composition;
using NLog;
using OdraIDE.Core.Services;

namespace OdraIDE.Core
{
    [Export(Logging.LoggingService, typeof(ILoggingService))]
    class NLogLoggingService : ILoggingService 
    {
        Logger log;

        public NLogLoggingService()
		{
            log = LogManager.GetCurrentClassLogger();
		}
		
		public void Debug(object message)
		{
			log.Debug(message);
		}

        public void DebugWithFormat(string format, params object[] args)
		{
            if (args.Length == 0)
            {
                log.Debug(format);
            }
            else
            {
                Debug(string.Format(format, args));
            }
		}
		
		public void Info(object message)
		{
			log.Info(message);
		}

        public void InfoWithFormat(string format, params object[] args)
		{
            if (args.Length == 0)
            {
                log.Info(format);
            }
            else
            {
                Info(string.Format(format, args));
            }
		}
		
		public void Warn(object message)
		{
			log.Warn(message);
		}
		
		public void Warn(object message, Exception exception)
		{
            log.WarnException(message.ToString(), exception);
		}

        public void WarnWithFormat(string format, params object[] args)
		{
            if (args.Length == 0)
            {
                log.Warn(format);
            }
            else
            {
                log.Warn(string.Format(format, args));
            }
		}
		
		public void Error(object message)
		{
			log.Error(message);
		}
		
		public void Error(object message, Exception exception)
		{
			log.ErrorException(message.ToString(), exception);
		}

        public void ErrorWithFormat(string format, params object[] args)
		{
            if (args.Length == 0)
            {
                log.Error(format);
            }
            else
            {
                log.Error(string.Format(format, args));
            }
		}
		
		public void Fatal(object message)
		{
			log.Fatal(message);
		}
		
		public void Fatal(object message, Exception exception)
		{
			log.FatalException(message.ToString(), exception);
		}

        public void FatalWithFormat(string format, params object[] args)
		{
            if (args.Length == 0)
            {
                log.Fatal(format);
            }
            else
            {
                log.Fatal(string.Format(format, args));
            }
		}
		
		public bool IsDebugEnabled {
			get {
				return log.IsDebugEnabled;
			}
		}
		
		public bool IsInfoEnabled {
			get {
				return log.IsInfoEnabled;
			}
		}
		
		public bool IsWarnEnabled {
			get {
				return log.IsWarnEnabled;
			}
		}
		
		public bool IsErrorEnabled {
			get {
				return log.IsErrorEnabled;
			}
		}
		
		public bool IsFatalEnabled {
			get {
				return log.IsFatalEnabled;
			}
		}
    }
}
