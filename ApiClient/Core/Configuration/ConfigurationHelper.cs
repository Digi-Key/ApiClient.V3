//-----------------------------------------------------------------------
//
// THE SOFTWARE IS PROVIDED "AS IS" WITHOUT ANY WARRANTIES OF ANY KIND, EXPRESS, IMPLIED, STATUTORY, 
// OR OTHERWISE. EXPECT TO THE EXTENT PROHIBITED BY APPLICABLE LAW, DIGI-KEY DISCLAIMS ALL WARRANTIES, 
// INCLUDING, WITHOUT LIMITATION, ANY IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, 
// SATISFACTORY QUALITY, TITLE, NON-INFRINGEMENT, QUIET ENJOYMENT, 
// AND WARRANTIES ARISING OUT OF ANY COURSE OF DEALING OR USAGE OF TRADE. 
// 
// DIGI-KEY DOES NOT WARRANT THAT THE SOFTWARE WILL FUNCTION AS DESCRIBED, 
// WILL BE UNINTERRUPTED OR ERROR-FREE, OR FREE OF HARMFUL COMPONENTS.
// 
//-----------------------------------------------------------------------

using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using ApiClient.Core.Configuration.Interfaces;
using Common.Logging;

namespace ApiClient.Core.Configuration
{
    /// <summary>
    ///     Helper class that wraps up working with System.Configuration.Configuration
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ConfigurationHelper : IConfigurationHelper
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ConfigurationHelper));

        /// <summary>
        ///     This object represents the config file
        /// </summary>
        protected System.Configuration.Configuration _config;

        /// <summary>
        ///     Updates the value for the specified key in the AppSettings of the Config file.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Update(string key, string value)
        {
            if (_config.AppSettings.Settings[key] == null)
            {
                _config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                _config.AppSettings.Settings[key].Value = value;
            }
        }

        /// <summary>
        ///     Gets the attribute or value of the key.
        /// </summary>
        /// <param name="attrName">Name of the attribute.</param>
        /// <returns>string value of attribute</returns>
        public string GetAttribute(string attrName)
        {
            try
            {
                return _config.AppSettings.Settings[attrName] == null
                    ? null
                    : _config.AppSettings.Settings[attrName].Value;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        ///     Gets the boolean attribute or value.
        /// </summary>
        /// <param name="attrName">Name of the attribute.</param>
        /// <returns>true of false</returns>
        public bool GetBooleanAttribute(string attrName)
        {
            try
            {
                var value = GetAttribute(attrName);
                return value != null && Convert.ToBoolean(value);
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Saves this instance.
        /// </summary>
        public void Save()
        {
            try
            {
                _config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (ConfigurationErrorsException cee)
            {
                _log.DebugFormat($"Exception Message {cee.Message}");
                throw;
            }
            catch (System.Exception ex)
            {
                _log.DebugFormat($"Exception Message {ex.Message}");
                throw;
            }
        }

        /// <summary>
        ///     Refreshes the application settingses.
        /// </summary>
        public void RefreshAppSettings()
        {
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
