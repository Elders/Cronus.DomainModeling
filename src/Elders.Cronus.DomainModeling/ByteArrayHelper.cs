using System.Runtime.InteropServices;

namespace Elders.Cronus
{
    public static class ByteArrayHelper
    {
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int memcmp(byte[] b1, byte[] b2, long count);

        public static bool Compare(byte[] b1, byte[] b2)
        {
            // Validate buffers are the same length.
            // This also ensures that the count does not exceed the length of either buffer.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return CompareUnsafe(b1, b2);
            }
            else
                return b1.Length == b2.Length && memcmp(b1, b2, b1.Length) == 0;
        }

        public unsafe static bool CompareUnsafe(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;
            int len = a.Length;
            unsafe
            {
                fixed (byte* ap = a, bp = b)
                {
                    long* alp = (long*)ap, blp = (long*)bp;
                    for (; len >= 8; len -= 8)
                    {
                        if (*alp != *blp) return false;
                        alp++;
                        blp++;
                    }
                    byte* ap2 = (byte*)alp, bp2 = (byte*)blp;
                    for (; len > 0; len--)
                    {
                        if (*ap2 != *bp2) return false;
                        ap2++;
                        bp2++;
                    }
                }
            }
            return true;
        }

        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] rv = new byte[first.Length + second.Length];
            System.Buffer.BlockCopy(first, 0, rv, 0, first.Length);
            System.Buffer.BlockCopy(second, 0, rv, first.Length, second.Length);
            return rv;
        }

        public static int ComputeHash(params byte[] data)
        {
            unchecked
            {
                const int p = 16777619;
                int hash = (int)2166136261;

                for (int i = 0; i < data.Length; i++)
                    hash = (hash ^ data[i]) * p;

                hash += hash << 13;
                hash ^= hash >> 7;
                hash += hash << 3;
                hash ^= hash >> 17;
                hash += hash << 5;
                return hash;
            }
        }
    }
}
