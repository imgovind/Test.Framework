using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Log.NLog
{
    public class NLogLogger : ILogger
    {
        private readonly Logger logger;

        public NLogLogger(Type loggerType)
        {
            Ensure.Argument.IsNotNull(loggerType, "loggerType");
            logger = LogManager.GetLogger(loggerType.FullName);
        }

        public void Trace(string message)
        {
            Ensure.Argument.IsNotNullOrEmpty(message, "message");
            logger.Trace(message);
        }

        public void Trace(string message, params object[] args)
        {
            Ensure.Argument.IsNotNullOrEmpty(message, "message");
            Ensure.Argument.IsNotNull(args, "args");
            logger.Trace(message, args);
        }

        public void Debug(string message)
        {
            Ensure.Argument.IsNotNullOrEmpty(message, "message");
            logger.Debug(message);
        }

        public void Debug(string message, params object[] args)
        {
            Ensure.Argument.IsNotNullOrEmpty(message, "message");
            Ensure.Argument.IsNotNull(args, "args");
            logger.Debug(message, args);
        }

        public void Info(string message)
        {
            Ensure.Argument.IsNotNullOrEmpty(message, "message");
            logger.Info(message);
        }

        public void Info(string message, params object[] args)
        {
            Ensure.Argument.IsNotNullOrEmpty(message, "message");
            Ensure.Argument.IsNotNull(args, "args");
            logger.Info(message, args);
        }

        public void Warning(string message)
        {
            Ensure.Argument.IsNotNullOrEmpty(message, "message");
            logger.Warn(message);
        }

        public void Warning(string message, params object[] args)
        {
            Ensure.Argument.IsNotNullOrEmpty(message, "message");
            Ensure.Argument.IsNotNull(args, "args");
            logger.Warn(message, args);
        }

        public void Error(string message)
        {
            Ensure.Argument.IsNotNullOrEmpty(message, "message");
            logger.Error(message);
        }

        public void Error(string message, params object[] args)
        {
            Ensure.Argument.IsNotNullOrEmpty(message, "message");
            Ensure.Argument.IsNotNull(args, "args");
            logger.Error(message, args);
        }

        public void Error(Exception exception, string message)
        {
            Ensure.Argument.IsNotNull(exception, "exception");
            Ensure.Argument.IsNotNullOrEmpty(message, "message");
            logger.Error(message, exception);
        }

        public void Error(Exception exception, string message, params object[] args)
        {
            Ensure.Argument.IsNotNull(exception, "exception");
            Ensure.Argument.IsNotNullOrEmpty(message, "message");
            Ensure.Argument.IsNotNull(args, "args");
            logger.Error(message.FormatWith(args), exception);
        }

        public void Fatal(string message)
        {
            Ensure.Argument.IsNotNullOrEmpty(message, "message");
            logger.Fatal(message);
        }

        public void Fatal(string message, params object[] args)
        {
            Ensure.Argument.IsNotNullOrEmpty(message, "message");
            Ensure.Argument.IsNotNull(args, "args");
            logger.Fatal(message, args);
        }

        public void Fatal(Exception exception, string message)
        {
            Ensure.Argument.IsNotNull(exception, "exception");
            Ensure.Argument.IsNotNullOrEmpty(message, "message");
            logger.Fatal(message, exception);
        }

        public void Fatal(Exception exception, string message, params object[] args)
        {
            Ensure.Argument.IsNotNull(exception, "exception");
            Ensure.Argument.IsNotNullOrEmpty(message, "message");
            Ensure.Argument.IsNotNull(args, "args");
            logger.Fatal(message.FormatWith(args), exception);
        }
    }
}
