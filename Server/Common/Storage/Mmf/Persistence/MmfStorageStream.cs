using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using CSharpTest.Net.Collections;
using CSharpTest.Net.Serialization;
using MutDood.Storage.Strategies.Mmf.Persistence;
using MutDood.Storage.Strategies.Mmf.Serializers;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;

namespace MUTDOD.Server.Common.Storage.Strategies.Mmf.Persistence
{
    public class MmfStorageStream : INodeStorage
    {
        static readonly ISerializer<IStorageHandle> StorageHandleSerializer = new HandleSerializer();
        private MemoryMappedFile _handle;
        private MemoryMappedViewStream _stream;
        private SegmentMap _map;


        private readonly IDatabaseParameters _databaseParameters;
        public MmfStorageStream(IDatabaseParameters databaseParameters)
        {
            _databaseParameters = databaseParameters;
            CreateHandles();
        }

        private void CreateHandles()
        {
            if (File.Exists(_databaseParameters.DataFileFullPath))
            {
                var fileInfo = new FileInfo(_databaseParameters.DataFileFullPath);

                _handle = MemoryMappedFile.CreateFromFile(_databaseParameters.DataFileFullPath,
                    FileMode.Open,
                    _databaseParameters.Name,
                    fileInfo.Length,
                    MemoryMappedFileAccess.ReadWrite);

            }
            else
            {
                _handle = MemoryMappedFile.CreateFromFile(_databaseParameters.DataFileFullPath,
                    FileMode.OpenOrCreate,
                    _databaseParameters.Name,
                    _databaseParameters.StartupSize,
                    MemoryMappedFileAccess.ReadWrite);
            }
            _stream = _handle.CreateViewStream();
            _map = SegmentMap.Read(_databaseParameters);
        }


        public void WriteTo(IStorageHandle value, Stream stream)
        {
            StorageHandleSerializer.WriteTo(value, stream);
        }

        public IStorageHandle ReadFrom(Stream stream)
        {
            return StorageHandleSerializer.ReadFrom(stream);
        }

        public void Dispose()
        {
            if (_stream != null)
            {
                _stream.Flush();
                _stream.Dispose();
            }

            if (_handle != null)
            {
                _handle.Dispose();
            }
            if (_map != null)
            {
                SegmentMap.Save(_map);
            }
        }

        public IStorageHandle OpenRoot(out bool isNew)
        {
            isNew = !_map.RecordIdMap.ContainsKey(RecordId.FirstIdentity.Id);
            if (isNew)
            {
                CreateRoot(); 
            }
            return RecordId.FirstIdentity;
        }

        private void CreateRoot()
        {
            var segments = _map.GetFreeSegments(1);
            _map.RecordIdMap.Add(RecordId.FirstIdentity.Id, segments);
        }

        public void Reset()
        {
            foreach (var keyValue in _map.RecordIdMap)
            {
                _map.SetSegmentsFree(keyValue.Value);
            }
            _map.RecordIdMap.Clear();
            CreateRoot();
        }

        public bool TryGetNode<TNode>(IStorageHandle handle, out TNode node, ISerializer<TNode> serializer)
        {
            var recordId = (handle as RecordId);
            if (recordId == null)
            {
                throw new ArgumentException("Handle type is not RecordId");
            }
            var segments = _map.RecordIdMap[recordId.Id];
            byte[] buffer = ReadBuffer(segments);
            node = serializer.FromByteArray(buffer);
            return true;
        }



        public IStorageHandle Create()
        {
            var recordId = new RecordId(_map.NextRecordId);
            var segments = _map.GetFreeSegments(1);
            _map.RecordIdMap.Add(recordId.Id, segments);
            return recordId;
        }

        public void Destroy(IStorageHandle handle)
        {
            var recordId = (handle as RecordId);
            if (recordId == null)
            {
                throw new ArgumentException("Handle type is not RecordId");
            }
            var segments = _map.RecordIdMap[recordId.Id];
            _map.SetSegmentsFree(segments);
            _map.RecordIdMap.Remove(recordId.Id);
        }

        public void Update<TNode>(IStorageHandle handle, ISerializer<TNode> serializer, TNode node)
        {
            var recordId = (handle as RecordId);
            if (recordId == null)
            {
                throw new ArgumentException("Handle type is not RecordId");
            }
            var segments = _map.RecordIdMap[recordId.Id];


            var buffer = serializer.ToByteArray(node);
            var length = buffer.Length;
            if (_map.GetSegmentsNeeded(length) != segments.Count)
            {
                _map.SetSegmentsFree(segments);
                segments = _map.GetFreeSegments(length);
                _map.RecordIdMap[recordId.Id] = segments;
            }
            WriteBuffer(buffer, segments);
        }

        private void WriteBuffer(byte[] buffer, List<int> segments)
        {
            var length = buffer.Length;
            for (int i = 0; i < segments.Count; i++)
            {
                var position = GetSegmentPosition(segments[i]);
                
                var toCopy = length > _databaseParameters.PageSize ? _databaseParameters.PageSize : length;
                
                _stream.Seek(position, SeekOrigin.Begin);
                _stream.Write(buffer, i*_databaseParameters.PageSize, toCopy);
                
                length -= _databaseParameters.PageSize;
            }
        }
        private byte[] ReadBuffer(List<int> segments)
        {
            segments.Sort();
            var buffer = new byte[segments.Count * _databaseParameters.PageSize];
            for (int i = 0; i < segments.Count; i++)
            {
                var position = GetSegmentPosition(segments[i]);
                var tmpArr = new byte[_databaseParameters.PageSize];
                _stream.Seek(position, SeekOrigin.Begin);
                _stream.Read(tmpArr, 0, _databaseParameters.PageSize);
                //_accessor.ReadArray(position, tmpArr, 0, _databaseParameters.PageSize);
                Buffer.BlockCopy(tmpArr, 0, buffer, i * _databaseParameters.PageSize, _databaseParameters.PageSize);
            }
            return buffer;
        }

        private int GetSegmentPosition(int segmentId)
        {
            return segmentId * _databaseParameters.PageSize;
        }
    }
}