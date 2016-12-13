// Skybot 2013-2016

using System;

namespace SkyBot
{
    public static class RNG
    {
        private static Random gen = new Random();

        public static int Next(int minValue, int maxValue)
        {
            return gen.Next(minValue, maxValue);
        }
    }
}
