﻿using System;
using System.Net.Http;

namespace Test.Framework.Api
{
    /// <summary>
    /// This contains the only method server Caching has
    /// </summary>
    public interface ICachingHandler
    {
        /// <summary>
        /// Invalidates the request. 
        /// </summary>
        /// <param name="request"></param>
        void InvalidateResource(HttpRequestMessage request);

        /// <summary>
        /// Generates cacheKey. Sometimes can be useful
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CacheKey GenerateCacheKey(HttpRequestMessage request);
    }
}