using System;
using System.IO;

namespace SharpIpp.Protocol.Extensions
{
    internal static class BinaryReaderExtensions
    {
        public static short ReadInt16BigEndian(this BinaryReader reader)
        {
            var value = reader.ReadInt16();

            if (BitConverter.IsLittleEndian)
                value = Bytes.Reverse(value);

            return value;
        }

        public static int ReadInt32BigEndian(this BinaryReader reader)
        {
            var value = reader.ReadInt32();

            if (BitConverter.IsLittleEndian)
                value = Bytes.Reverse(value);

            return value;
        }
    }
}
