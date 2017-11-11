using System;
using System.IO;
using System.Runtime.InteropServices;
using CSharpTest.Net.Serialization;

namespace MutDood.Storage.Strategies.Mmf.Serializers
{
    public class ValueSerializer : ISerializer<byte[]>
    {
        private readonly int _intSize = Marshal.SizeOf(typeof(int));
        public void WriteTo(byte[] value, Stream stream)
        {
            var length = value.Length;
            var lengthBuffer = BitConverter.GetBytes(length);
            //byte[] toSave = new byte[_intSize + length];

            //Buffer.BlockCopy(lengthBuffer, 0, toSave, 0, _intSize);
            //Buffer.BlockCopy(value, 0, toSave, _intSize, length);
            
            stream.Write(lengthBuffer, 0, _intSize);
            stream.Write(value, 0, length);

        }

        public byte[] ReadFrom(Stream stream)
        {
            var lengthBuffer = new byte[_intSize];
            stream.Read(lengthBuffer, 0, _intSize);
            var length = BitConverter.ToInt32(lengthBuffer,0);
            var data = new byte[length];
            stream.Read(data, 0, length);
            return data;
        }
    }
}