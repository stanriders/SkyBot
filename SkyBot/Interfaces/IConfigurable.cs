// Skybot 2013-2017

using System.Collections.Generic;

namespace SkyBot
{
    abstract public class IConfigurable
    {
        public List<Configurable> Configurables = new List<Configurable>();

        public IConfigurable()
        {
            SetupConfig();
        }
        public virtual void SetupConfig() { }
    }
}
