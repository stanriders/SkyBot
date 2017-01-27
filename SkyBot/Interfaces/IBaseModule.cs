// Skybot 2013-2017

using System.Collections.Generic;

namespace SkyBot
{
    abstract public class IBaseModule
    {
        public List<Configurable> Configurables = new List<Configurable>();

        public IBaseModule()
        {
            SetupConfig();
        }
        public virtual void SetupConfig() { }

        public virtual void Cleanup() { }
    }
}
