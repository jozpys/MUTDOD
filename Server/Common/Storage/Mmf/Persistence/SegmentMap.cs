using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using MUTDOD.Common.ModuleBase.Storage.Core.Metadata;

namespace MUTDOD.Server.Common.Storage.Strategies.Mmf.Persistence
{
    [Serializable]
    public class SegmentMap
    {
        private int _nextRecordId = 1000;
        public int NextRecordId
        {
            get
            {
                Interlocked.Increment(ref _nextRecordId);
                return _nextRecordId;
            }
        }
        private Dictionary<int, List<int>> _recordIdMap;
        public Dictionary<int, List<int>> RecordIdMap { get { return _recordIdMap; }set { _recordIdMap = value; } }

        /// <summary>
        /// Lista wolnych segmentów przed <see cref="EndingSegment"/>
        /// </summary>
        private readonly List<int> _freeSegments;

        private readonly int _segmentSize;
        private readonly IDatabaseParameters _databaseParameters;

        /// <summary>
        /// Pierwszy wolny segment na końcu pliku
        /// </summary>
        private int _endingSegment = 10;

        

        public SegmentMap(IDatabaseParameters databaseParameters)
        {
            _freeSegments = new List<int>();
            _databaseParameters = databaseParameters;
            _segmentSize = databaseParameters.PageSize;
            _recordIdMap = new Dictionary<int, List<int>>();
        }

        public List<int> GetFreeSegments(int dataSize)
        {
            var outList = new List<int>();
            var neededSegments = (int)Math.Ceiling((double) dataSize/_segmentSize);

            if (_databaseParameters.OptimizeSize)
            {
                if (_freeSegments.Count >= neededSegments)
                {
                    var segments = _freeSegments.GetRange(0, neededSegments);
                    SetSegmentsOccupied(segments);
                    outList.AddRange(segments);
                    return outList;
                }
            }
            
            for (int i = 0; i < neededSegments; i++)
            {
                SetSegmentOccupied(_endingSegment);
                outList.Add(_endingSegment);
                _endingSegment++;
            }

            return outList;
        }

        private void SetSegmentsOccupied(List<int> segments)
        {
            foreach (var segment in segments)
            {
                SetSegmentOccupied(segment);
            }
        }

        public void SetSegmentOccupied(int segmentId)
        {
            if (_freeSegments.Contains(segmentId))
            {
                _freeSegments.Remove(segmentId); 
            }
        }

        public void SetSegmentFree(int segmentId)
        {
            _freeSegments.Add(segmentId);
            _freeSegments.Sort();
        }


        public static void Save(SegmentMap map)
        {
            var segmentFileName = Path.Combine(map._databaseParameters.BaseDirectory, String.Format("{0}.seg", map._databaseParameters.Name));
            using (var stream = new FileStream(segmentFileName, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, map);
            }
        }
        public static SegmentMap Read(IDatabaseParameters databaseParameters)
        {
            var segmentFileName = Path.Combine(databaseParameters.BaseDirectory, String.Format("{0}.seg",databaseParameters.Name));
            if (!File.Exists(segmentFileName))
            {
                return new SegmentMap(databaseParameters);
            }

            using (var stream = new FileStream(segmentFileName,FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(stream) as SegmentMap;
            }
        }

        public void SetSegmentsFree(List<int> segments)
        {
            foreach (var segment in segments)
            {
                SetSegmentFree(segment);
            }
        }

        public int GetSegmentsNeeded(int length)
        {
            return Convert.ToInt32(Math.Ceiling((double) length/_databaseParameters.PageSize));
        }
    }
}