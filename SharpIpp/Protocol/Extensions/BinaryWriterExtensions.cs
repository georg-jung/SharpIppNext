using System;
using System.IO;

namespace SharpIpp.Protocol.Extensions
{
    internal static class BinaryWriterExtensions
    {
        public static void WriteBigEndian(this BinaryWriter writer, short value)
        {
            if (BitConverter.IsLittleEndian)
                value = Bytes.Reverse(value);

            writer.Write(value);
        }

        public static void WriteBigEndian(this BinaryWriter writer, int value)
        {
            if (BitConverter.IsLittleEndian)
                value = Bytes.Reverse(value);

            writer.Write(value);
        }
    }
}
