using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ProjektB.Web.ConfigSections
{
    public class AuthApplicationCollection : ConfigurationElementCollection
    {
        public AuthApplicationCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new AuthApplication();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((AuthApplication)element).Type;
        }

        public AuthApplication this[int index]
        {
            get
            {
                return (AuthApplication)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public AuthApplication this[string Name]
        {
            get
            {
                return (AuthApplication)BaseGet(Name);
            }
        }

        public int IndexOf(AuthApplication auth)
        {
            return BaseIndexOf(auth);
        }

        public void Add(AuthApplication auth)
        {
            BaseAdd(auth);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(AuthApplication auth)
        {
            if (BaseIndexOf(auth) >= 0)
                BaseRemove(auth.Type);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
}