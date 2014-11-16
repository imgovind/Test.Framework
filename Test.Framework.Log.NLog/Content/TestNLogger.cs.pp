using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Log;
using Test.Framework.Log.NLog;

namespace $rootnamespace$
{
    public static class TestNLogger
    {
        public static void Start()
        {
            AppInitalizer.Core.Initialize();
            var logger = Container.ResolveOrRegister<ILogger, NLogLogger>(new NLogLogger(true));
            try
            {
                DateTime startTimer = DateTime.Now;
                for (int i = 0; i < 1; i++)
                {
                    logger.Debug("We're going to throw an exception now.");
                    logger.Warning("It's gonna happen!!");
                }
                throw new ApplicationException();
            }
            catch (ApplicationException ae)
            {
                logger.Error("Error doing something...", ae);
            }
        }
    }
}
