using System;
using System.IO;
using System.Runtime.InteropServices;
using CSharpTest.Net.Serialization;
using MUTDOD.Common.Types;

namespace MUTDOD.Server.Common.Storage.Strategies.Mmf.Serializers
{
    public class KeySerializer : ISerializer<Oid>
    {
        private readonly int _uintSize = Marshal.SizeOf(typeof(uint));
        private readonly int _guidSize = Marshal.SizeOf(typeof(Guid));
        public void WriteTo(Oid value, Stream stream)
        {
            var oli = BitConverter.GetBytes(value.Oli);
            var uid = value.Id.ToByteArray();
            stream.Write(oli, 0, oli.Length);
            stream.Write(uid, 0, uid.Length);
        }

        public Oid ReadFrom(Stream stream)
        {

            byte[] oliBuffer = new byte[_uintSize];
            byte[] uidBuffer = new byte[_guidSize];
            stream.Read(oliBuffer, 0, _uintSize);
            stream.Read(uidBuffer, 0, _guidSize);

            var oid = new Oid(new Guid(uidBuffer), BitConverter.ToUInt32(oliBuffer, 0));
            return oid;
        }
    }
}