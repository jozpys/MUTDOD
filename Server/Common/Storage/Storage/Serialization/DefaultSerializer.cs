using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using MUTDOD.Common.ModuleBase.Storage;
using MUTDOD.Common.ModuleBase.Storage.Core;
using MUTDOD.Common.Types;
using MUTDOD.Server.Common.Storage.MetadataElements;

namespace MUTDOD.Server.Common.Storage.Serialization
{
    public class DefaultSerializer : ISerializer
    {
        private readonly int _longSize = Marshal.SizeOf(typeof (long));
        private readonly int _intSize = Marshal.SizeOf(typeof(int));
        private StorageMetadata _metadata;


        private DefaultSerializer(StorageMetadata metadata)
        {
            _metadata = metadata;
        }

        public static ISerializer Create(StorageMetadata metadata)
        {
            return new DefaultSerializer(metadata);
        }

        public List<SerializedStorable> Serialize(IStorable objectToStore)
        {
            var list = new List<SerializedStorable>();
            var ms = new MemoryStream();
            foreach (var property in objectToStore.Properties)
            {
                var propertyValue = SerializeValueType(property.Key, property.Value);
                var propertyFullData = GetPreparedPropertyData(property.Key, propertyValue);
                ms.Write(propertyFullData, 0, propertyFullData.Length);

            }
            list.Add(new SerializedStorable { Oid = objectToStore.Oid, Data = ms.ToArray() });
            ms.Dispose();
            return list;
        }

        private byte[] GetPreparedPropertyData(Property property, byte[] propertyValue)
        {
            var idBytes = BitConverter.GetBytes(property.PropertyId.Id);
            var lengthBytes = BitConverter.GetBytes(propertyValue.Length);
            byte[] output = new byte[idBytes.Length + propertyValue.Length + lengthBytes.Length];
            
            Buffer.BlockCopy(idBytes,0,output,0,idBytes.Length);
            Buffer.BlockCopy(lengthBytes,0,output,idBytes.Length,lengthBytes.Length);
            Buffer.BlockCopy(propertyValue,0,output,idBytes.Length+lengthBytes.Length,propertyValue.Length);
            return output;
        }

        private byte[] SerializeValueType(Property property, object value)
        {
            var bytes = Encoding.UTF8.GetBytes(value.ToString());
            return bytes;
        }

        private object DeserializeValueType(Property property, byte[] value)
        {
            var s= Encoding.UTF8.GetString(value);
            var obj = TypeDescriptor.GetConverter(property.DotNetType).ConvertFromString(s);
            return obj;
        }

        public IStorable Deserialize(Did dbId, SerializedStorable serializedStorable)
        {
            var storable = new Storable();
            storable.Oid = serializedStorable.Oid;

            int streamLength = serializedStorable.Data.Length;
            int position = 0;
            while (position < streamLength)
            {
                var id = BitConverter.ToInt64(serializedStorable.Data, position);
                var length = BitConverter.ToInt32(serializedStorable.Data, position+_longSize);
                var data = new byte[length];
                Buffer.BlockCopy(serializedStorable.Data, position+_longSize + _intSize, data, 0, length);
                position += _longSize + _intSize + length;

                var property = _metadata.Databases[dbId].Schema.Properties.Single(p => p.Key.Id == id).Value;
                storable.Properties.Add(property, DeserializeValueType(property,data)); 
            }

            return storable;
        }
    }
}