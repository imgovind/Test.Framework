using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Cache
{
    public static class Cacher
    {
        private static ICacher _InProc;
        private static ICacher _OutProc;

        public static ICacher InProc { 
            get 
            { 
                if (_InProc == null) 
                    throw new Exception("InProc cacher not Initialized");
                return _InProc;
            } 
        }

        public static ICacher OutProc {
            get
            {
                if (_OutProc == null)
                    throw new Exception("OutProc cacher not initialized");
                return _OutProc;
            }
        }

        public static void InitializeInProc(ICacher inproCacher)
        {
            _InProc = inproCacher;
        }

        public static void InitializeOutProc(ICacher outproCacher)
        {
            _OutProc = outproCacher;
        }
    }
}
