using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ProjektB.Web.ConfigSections
{
    public class AuthSection : ConfigurationSection
    {
        [ConfigurationProperty("applications", IsRequired = true)]
        [ConfigurationCollection(typeof(AuthApplicationCollection),
        AddItemName = "add",
        ClearItemsName = "clear",
        RemoveItemName = "remove")]
        public AuthApplicationCollection Applications
        {
            get { return (AuthApplicationCollection)this["applications"]; }
        }
    }


    public class AuthApplication : ConfigurationElement
    {
        [ConfigurationProperty("type", DefaultValue = AuthType.None, IsRequired = true)]
        public AuthType Type
        {
            get { return (AuthType)this["type"]; }
            set { this["type"] = value; }
        }

        [ConfigurationProperty("appId", IsRequired = true)]
        public string AppId
        {
            get { return (string)this["appId"]; }
            set { this["appId"] = value; }
        }

        [ConfigurationProperty("appKey", IsRequired = true)]
        public string appKey
        {
            get { return (string)this["appKey"]; }
            set { this["appKey"] = value; }
        }
    }

        public enum AuthType
    {
        None,
        Facebook,
        Google
    }
}