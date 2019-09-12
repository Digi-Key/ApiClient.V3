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

namespace ApiClient.Core.Configuration.Interfaces
{
    /// <summary>
    ///     Interface for System.Configuration.Configuration helper class
    /// </summary>
    public interface IConfigurationHelper
    {
        /// <summary>
        ///     Updates the value for the specified key in the AppSettings of the Config file.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void Update(string key, string value);

        /// <summary>
        ///     Gets the attribute or value of the key.
        /// </summary>
        /// <param name="attrName">Name of the attribute.</param>
        /// <returns>string value of attribute</returns>
        string GetAttribute(string attrName);

        /// <summary>
        ///     Gets the boolean attribute or value.
        /// </summary>
        /// <param name="attrName">Name of the attribute.</param>
        /// <returns>true or false</returns>
        bool GetBooleanAttribute(string attrName);

        /// <summary>
        ///     Saves changes to the Config file
        /// </summary>
        void Save();

        /// <summary>
        ///     Refreshes the application settingses.
        /// </summary>
        void RefreshAppSettings();
    }
}
