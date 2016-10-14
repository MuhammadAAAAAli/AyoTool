using System;

namespace ayo.Static
{
    public class RandomNumber
    {
        private static readonly Random getRandom = new Random();
        private static readonly object syncLock = new object();

        public static int Get(int min, int max)
        {
            lock (syncLock)
            {
                return getRandom.Next(min, max);
            }
        }
    }
}