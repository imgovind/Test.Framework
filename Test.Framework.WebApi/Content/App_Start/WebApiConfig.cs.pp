using LightInject;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Test.Framework;
using Test.Framework.Extensions;
using Test.Framework.Handlers.Compression;
using Test.Framework.Handlers.Https;
using Test.Framework.Handlers.Resource;
using Test.Framework.WebApi;

namespace $rootnamespace$
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            WebApiRouteConfig.DefaultRoute(config);            

            ConfigureMediaFormatters(config);

            ConfigureIoc(config);

            ConfigureCache(config);

            ConfigureVersioning(config);

            ConfigureMessageHandlers(config);

            ConfigureResponseEnrichers(config);
        }

        private static void ConfigureResponseEnrichers(HttpConfiguration config)
        {
            if (!WebApiConstants.WebApi.EnrichResponse)
                return;
        }

        private static void ConfigureMessageHandlers(HttpConfiguration config)
        {
            if (WebApiConstants.WebApi.CompressResponse)
                config.MessageHandlers.Add(new CompressionHandler());

            if (WebApiConstants.WebApi.DecompressRequest)
                config.MessageHandlers.Add(new DecompressionHandler());

            if (WebApiConstants.WebApi.EnrichResponse)
                config.MessageHandlers.Add(new EnrichingHandler());

            if (WebApiConstants.WebApi.RequireHttps)
                config.MessageHandlers.Add(new RequireHttpsHandler());
        }

        private static void ConfigureVersioning(HttpConfiguration config)
        {
            if (WebApiConstants.WebApi.EnableVersioning)
                config.Services.Replace(typeof(IHttpControllerSelector), new CustomHttpControllerSelector(config));
        }

        private static void ConfigureIoc(HttpConfiguration config)
        {
            var container = (ServiceContainer)Container.IocContainer.GetUnderlyingContainer();
            container.RegisterApiControllers();
            container.EnableWebApi(config);
        }

        private static void ConfigureCache(HttpConfiguration config)
        {
            if (WebApiConstants.WebApi.EnableOutProcCaching)
            {
                var options = new ConfigurationOptions();
                options.EndPoints.Add("localserver");
                options.Password = "23Eiyztygt";
                var connection = ConnectionMultiplexer.Connect(options);

                var redisEntityTagStore = new RedisEntityTagStore(connection);
                var redisCachingHandler = new CachingHandler(config, redisEntityTagStore, new string[] { "Accept" });
                config.MessageHandlers.Add(redisCachingHandler);
            }

            if (WebApiConstants.WebApi.EnableInProcCaching)
            {
                var cachingHandler = new CachingHandler(config, new string[] { "Accept" });
                config.MessageHandlers.Add(cachingHandler);
            }
        }

        private static void ConfigureMediaFormatters(HttpConfiguration config)
        {
            if (WebApiConstants.WebApi.EnableXmlFormatting)
            {
                var xmlFormatter = config.Formatters.XmlFormatter;
                xmlFormatter.UseXmlSerializer = true;
                xmlFormatter.Indent = true;
            }

            if (WebApiConstants.WebApi.EnableJsonFormatting)
            {
                var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
                jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                CreateMediaTypes(jsonFormatter);
            }
        }

        private static void CreateMediaTypes(JsonMediaTypeFormatter jsonFormatter)
        {
            var mediaTypes = new string[] {
                "application/vnd.testdemo.v1+json",
                "application/vnd.testdemo.v2+json",
                "application/vnd.testdemo.v3+json"
            };

            foreach (var mediaType in mediaTypes)
            {
                jsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue(mediaType));
            }
        }
    }
}
