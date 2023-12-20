﻿// Even though .NET Standard 2.0 seems to support the ConfigurationManager class according
// to the NuGet package at https://www.nuget.org/packages/System.Configuration.ConfigurationManager,
// it will often throw a PlatformNotSupport exception. See
// https://docs.microsoft.com/en-us/dotnet/api/system.configuration.configurationmanager?view=netframework-4.8

using System.Configuration;

namespace FluentAssertionsAsync.Common;

internal class AppSettingsConfigurationStore : IConfigurationStore
{
    public string GetSetting(string name)
    {
        string value = ConfigurationManager.AppSettings[name];
        return !string.IsNullOrEmpty(value) ? value : null;
    }
}
