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

using ApiClient.Core.Configuration.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace ApiClient.Core.Configuration
{
    /// <summary>
    ///     Helper class that wraps up working with System.Configuration.Configuration
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ConfigurationHelper : IConfigurationHelper
    {
        /// <summary>
        ///     This object represents the config file
        /// </summary>
        protected XElement? _xconfig;

        /// <summary>
        ///     Updates the value for the specified key in the AppSettings of the Config file.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Update(string key, string value)
        {
            if (Setting(key) == null)
                AppSettings?.Add(CreateElement(key, value));
            else
                Value(Setting(key))!.SetValue(value);
        }

        public XElement? AppSettings { get => _xconfig?.Descendants("appSettings")?.FirstOrDefault(); }

        public IEnumerable<XElement>? Settings { get => AppSettings?.Descendants("add"); }

        public static XElement CreateElement(string key, string value)
        {
            return new XElement("add", new XAttribute("key", key), new XAttribute("value", value));
        }

        public XElement? Setting(string name)
        {
            return Settings?.Where(e => e.Attributes().Where(r => r.Name == "key" && r.Value == name).Any()).FirstOrDefault();
        }

        public static XAttribute? Value(XElement? setting)
        {
            return setting?.Attributes().Where(l => l.Name == "value").FirstOrDefault();
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
                return Value(Setting(attrName))?.Value!;
            }
            catch (System.Exception)
            {
                return null!;
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
            _xconfig!.Save(ApiClientConfigHelper.ConfigFile);
        }
    }
}
