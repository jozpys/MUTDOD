using System;
using System.Xml.Serialization;

namespace IndexMechanism.IndexManager
{
    internal delegate void StatisticsChanged();

    [Serializable]
    public struct StatisticInfo
    {
        [XmlAttribute] public float Optimistic;
        [XmlAttribute] public float Pessimistic;
        [XmlAttribute] public float Average;
        [XmlAttribute] public float AverageCount;

        internal void include(float secounds)
        {
            if (AverageCount == 0)
            {
                Optimistic = secounds;
                Pessimistic = secounds;
            }
            else if (Optimistic > secounds)
                Optimistic = secounds;
            else if (Pessimistic < secounds)
                Pessimistic = secounds;

            Average = (((float) AverageCount*Average) + secounds)/++AverageCount;
            if (float.IsNaN(Average))
                Average = float.MinValue;
            if (float.IsInfinity(Average))
                Average = float.MaxValue;
        }

        internal void resetStatistics()
        {
            AverageCount = 0;
        }
    }

    [Serializable]
    public class IndexStatistics
    {
        [XmlElement] public StatisticInfo ObjectIndexing;
        [XmlElement] public StatisticInfo ObjectIndexRefresh;
        [XmlElement] public StatisticInfo ObjectIndexRemove;
        [XmlElement] public StatisticInfo ObjectSearch;
        [XmlElement] public StatisticInfo HitRatio;
        [XmlElement] public StatisticInfo RoleIndexing;
        [XmlElement] public StatisticInfo RoleIndexRefresh;
        [XmlElement] public StatisticInfo RoleIndexRemove;
        [XmlElement] public StatisticInfo RoleSearch;

        #region include methods

        internal void includeObjectIndexed(float secounds)
        {
            ObjectIndexing.include(secounds);

            StatisticsChanged();
        }

        internal void includeObjectIndexRefresh(float secounds)
        {
            ObjectIndexRefresh.include(secounds);
            StatisticsChanged();
        }

        internal void includeObjectRemoved(float secounds)
        {
            ObjectIndexRemove.include(secounds);
            StatisticsChanged();
        }

        internal void includeObjectSearch(float secounds)
        {
            ObjectSearch.include(secounds);
            StatisticsChanged();
        }

        internal void includeRoleIndexed(float secounds)
        {
            RoleIndexing.include(secounds);

            StatisticsChanged();
        }

        internal void includeRoleIndexRefresh(float secounds)
        {
            RoleIndexRefresh.include(secounds);
            StatisticsChanged();
        }

        internal void includeRoleRemoved(float secounds)
        {
            RoleIndexRemove.include(secounds);
            StatisticsChanged();
        }

        internal void includeRoleSearch(float secounds)
        {
            RoleSearch.include(secounds);
            StatisticsChanged();
        }

        internal void includeHitRatio(float ratio)
        {
            HitRatio.include(ratio);
            StatisticsChanged();
        }

        #endregion

        internal void resetStstistics()
        {
            ObjectIndexing.resetStatistics();
            ObjectIndexRefresh.resetStatistics();
            ObjectIndexRemove.resetStatistics();
            ObjectSearch.resetStatistics();
            HitRatio.resetStatistics();
            RoleIndexing.resetStatistics();
            RoleIndexRefresh.resetStatistics();
            RoleIndexRemove.resetStatistics();
            RoleSearch.resetStatistics();
        }

        internal StatisticsChanged StatisticsChangedHandler;

        private void StatisticsChanged()
        {
            if (StatisticsChangedHandler != null)
                StatisticsChangedHandler();
        }
    }
}