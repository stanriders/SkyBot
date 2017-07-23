// Skybot 2013-2017

using System.Collections.Generic;
using System.Linq;

namespace SkyBot
{
    public static class Converters
    {
        public static List<string> GetProperties<T>()
        {
            var propList = new List<string>();

            foreach (var sourceProperty in typeof(T).GetProperties())
            {
                if (!sourceProperty.CanRead || (sourceProperty.GetIndexParameters().Length > 0))
                    continue;

                propList.Add(sourceProperty.Name);
            }

            return propList;
        }

        public static void SetProperties<T>(T data, Dictionary<string, object> dictionary)
        {
            foreach (var sourceProperty in typeof(T).GetProperties())
            {
                if (!dictionary.Keys.Any(x => x == sourceProperty.Name))
                    continue;

                if (!sourceProperty.CanRead || (sourceProperty.GetIndexParameters().Length > 0))
                    continue;

                if ((sourceProperty != null) && (sourceProperty.CanWrite))
                    sourceProperty.SetValue(data, dictionary[sourceProperty.Name], null);
            }
        }
    }
}
