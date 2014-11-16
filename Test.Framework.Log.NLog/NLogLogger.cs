using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Extensions;

namespace Test.Framework.Log.NLog
{
    public class NLogLogger : ILogger
    {
        private readonly Logger logger;

        public NLogLogger()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        public NLogLogger(bool typeNameGiven, string memberName = "")
        {
            if (memberName.IsNullOrEmpty())
            {
                StackFrame frame = new StackFrame(1);
                if (frame != null)
                {
                    var method = frame.GetMethod();
                    if (method != null)
                    {
                        var type = method.DeclaringType;
                        memberName = type.FullName;
                    }
                }
            }
            logger = LogManager.GetLogger(memberName);
        }

        //public NLogLogger(Type loggerType)
        //{
        //    Ensure.Argument.IsNotNull(loggerType, "loggerType");
        //    logger = LogManager.GetLogger(loggerType.FullName);
        //}

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
