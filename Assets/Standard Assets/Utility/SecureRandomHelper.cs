using System;
using System.Security.Cryptography;

namespace UnityStandardAssets.Utility
{
    public static class SecureRandomHelper
    {
        private static readonly RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        /// <summary>
        /// Returns a cryptographically secure random integer that is within a specified range.
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number returned.</param>
        /// <param name="max">The exclusive upper bound of the random number returned. max must be greater than or equal to min.</param>
        /// <returns>A 32-bit signed integer greater than or equal to min and less than max.</returns>
        public static int Range(int min, int max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException("min", "min must be less than or equal to max");
            if (min == max)
                return min;

            long range = (long)max - min;
            byte[] data = new byte[4];
            uint value;

            long maxRandom = (long)uint.MaxValue + 1;
            long remainder = maxRandom % range;

            do
            {
                rng.GetBytes(data);
                value = BitConverter.ToUInt32(data, 0);
            } while (value >= maxRandom - remainder);

            return (int)(min + (value % range));
        }
    }
}
