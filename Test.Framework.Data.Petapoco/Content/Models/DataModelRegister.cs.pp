﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Data;
using Test.Framework.Extensions;
using $rootnamespace$.Models;

namespace $rootnamespace$.Models
{
    public static class DataModelRegister
    {
        static DataModelRegister()
        {
        }

        public static void Register()
        {
            Initialize(typeof(IDataModel));
        }

        public static void Initialize(Type type)
        {
            Ensure.Argument.IsNotNull(type, "type");

            var currentAssemblyTypes = AppDomain.CurrentDomain.GetAssemblies()
                .ToList()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).ToList();

            currentAssemblyTypes.ForEach(t =>
            {
                PropertyCache.Register(t);
            });
        }
    }
}
