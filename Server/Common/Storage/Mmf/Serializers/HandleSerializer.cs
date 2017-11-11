using System;
using System.IO;
using System.Runtime.InteropServices;
using CSharpTest.Net.Collections;
using CSharpTest.Net.Serialization;
using MutDood.Storage.Strategies.Mmf.Persistence;

namespace MutDood.Storage.Strategies.Mmf.Serializers
{
    internal class HandleSerializer : ISerializer<IStorageHandle>
    {
        public void WriteTo(IStorageHandle value, Stream stream)
        {
            var handle = value as RecordId;
            if (handle == null)
            {
                throw new ArgumentException("Handle is not RecordId");
            }
            var buffer = BitConverter.GetBytes(handle.Id);
            stream.Write(buffer, 0, buffer.Length);
        }

        public IStorageHandle ReadFrom(Stream stream)
        {
            var buffer = new byte[Marshal.SizeOf(typeof(int))];
            stream.Read(buffer, 0, buffer.Length);
            return RecordId.Recreate(BitConverter.ToInt32(buffer, 0));
        }
    }
}