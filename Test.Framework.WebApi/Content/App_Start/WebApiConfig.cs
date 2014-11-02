using LightInject;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace Test.Framework.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            //config.RegisterDefaultRoutes();

            ConfigureMediaFormatters(config);

            ConfigureIoc(config);

            ConfigureCache(config);

            ConfigureVersioning(config);

            ConfigureMessageHandlers(config);

            ConfigureResponseEnrichers(config);
        }

        private static void ConfigureResponseEnrichers(HttpConfiguration config)
        {
        }

        private static void ConfigureMessageHandlers(HttpConfiguration config)
        {
            //config.MessageHandlers.Add(new CompressionHandler());
            //config.MessageHandlers.Add(new DecompressionHandler());
            //config.MessageHandlers.Add(new GZipToJsonHandler());
            //config.MessageHandlers.Add(new EnrichingHandler());
        }

        private static void ConfigureVersioning(HttpConfiguration config)
        {
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
            //var options = new ConfigurationOptions();
            //options.EndPoints.Add("localserver");
            //options.Password = "23Eiyztygt";
            //var connection = ConnectionMultiplexer.Connect(options);

            //var redisEntityTagStore = new RedisEntityTagStore(connection);
            //var redisCachingHandler = new CachingHandler(config, redisEntityTagStore, new string[] { "Accept" });
            //config.MessageHandlers.Add(redisCachingHandler);

            var cachingHandler = new CachingHandler(config, new string[] { "Accept" });
            config.MessageHandlers.Add(cachingHandler);
        }

        private static void ConfigureMediaFormatters(HttpConfiguration config)
        {
            var xmlFormatter = config.Formatters.XmlFormatter;
            xmlFormatter.UseXmlSerializer = true;
            xmlFormatter.Indent = true;

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            CreateMediaTypes(jsonFormatter);
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
