﻿using Microsoft.Extensions.Configuration;
using System;

namespace Web.Config.Transform.Buildpack
{
    public class ConfigurationTracer : ITracer
    {
        private readonly IConfigurationRoot _configuration;
        private readonly IEnvironmentWrapper environmentWrapper;
        private readonly ILogger _logger;

        public ConfigurationTracer(IEnvironmentWrapper environmentWrapper, IConfigurationFactory configurationFactory, ILogger logger)
        {
            _configuration = configurationFactory.GetConfiguration(environmentWrapper.GetEnvironmentVariable(Constants.ASPNETCORE_ENVIRONMENT_NM) ?? "Release");
            this.environmentWrapper = environmentWrapper;
            _logger = logger;
        }

        public void FlushEnvironmentVariables()
        {
            if (IsTraceConfigEnabled())
            {
                _logger.WriteLog($"-----> TRACE: Flushing out configurations...");
                foreach (var config in _configuration.AsEnumerable())
                {
                    _logger.WriteLog($"-----> TRACE: KEY=> {config.Key}, VALUE=> {config.Value}");
                }
            }
        }

        public bool IsTraceConfigEnabled()
        {
            return Convert.ToBoolean(environmentWrapper.GetEnvironmentVariable(Constants.TRACE_CONFIG_ENABLED_NM) ?? "false")
                && (environmentWrapper.GetEnvironmentVariable(Constants.ASPNETCORE_ENVIRONMENT_NM) ?? "Release").ToLower().Contains("dev");
        }
    }
}
