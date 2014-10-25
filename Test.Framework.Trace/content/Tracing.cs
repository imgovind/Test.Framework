using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Trace
{
    public static class Tracing
    {
        private static TraceSource _ts = new TraceSource("GeneralTrace");

        public static void Start(string message)
        {
            TraceEvent(TraceEventType.Start, message);
        }

        public static void Stop(string message)
        {
            TraceEvent(TraceEventType.Stop, message);
        }

        public static void Information(string message)
        {
            TraceEvent(TraceEventType.Information, message);
        }

        public static void InformationFormat(string message, params object[] objects)
        {
            TraceEventFormat(TraceEventType.Information, message, objects);
        }

        public static void Warning(string message)
        {
            TraceEvent(TraceEventType.Warning, message);
        }

        public static void WarningFormat(string message, params object[] objects)
        {
            TraceEventFormat(TraceEventType.Warning, message, objects);
        }

        public static void Error(string message)
        {
            TraceEvent(TraceEventType.Error, message);
        }

        public static void ErrorVerbose(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            TraceEventFormat(TraceEventType.Error, "{0}\n\nMethod: {1}\nFilename: {2}\nLine number: {3}", message, memberName, filePath, lineNumber);
        }

        public static void ErrorFormat(string message, params object[] objects)
        {
            TraceEventFormat(TraceEventType.Error, message, objects);
        }

        public static void Verbose(string message)
        {
            TraceEvent(TraceEventType.Verbose, message);
        }

        public static void VerboseFormat(string message, params object[] objects)
        {
            TraceEventFormat(TraceEventType.Verbose, message, objects);
        }

        public static void Transfer(string message, Guid activity)
        {
            _ts.TraceTransfer(0, message, activity);
        }

        public static void TraceEventFormat(TraceEventType type, string message, params object[] objects)
        {
            var format = string.Format(message, objects);
            TraceEvent(type, format);
        }

        public static void TraceEvent(TraceEventType type, string message)
        {
            if (System.Diagnostics.Trace.CorrelationManager.ActivityId == Guid.Empty)
            {
                System.Diagnostics.Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            }

            _ts.TraceEvent(type, 0, message);
        }
    }

}
