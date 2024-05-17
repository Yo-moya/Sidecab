
using System;
using System.Collections.Generic;

namespace Sidecab.Model
{
    //==========================================================================
    public class LocationHistory
    {
        //----------------------------------------------------------------------
        public int MaxFreshness
        {
            get => _maxFreshness;
            set => _maxFreshness = Math.Max(1, value);
        }

        //----------------------------------------------------------------------
        public int MaxHistories
        {
            get => _maxHistories;
            set => _maxHistories = Math.Max(1, value);
        }


        private HashSet<Item> _historyList = [];
        private int _maxFreshness = 10;
        private int _maxHistories = 10;


        //----------------------------------------------------------------------
        public void NotifyFolder(Folder folder)
        {
            // Age items
            foreach (var i in _historyList)
            {
                i.Freshness = Math.Max(0, i.Freshness - 1);
                i.Folder.InvokeFreshnessUpdateEvent();
            }

            Item item = new(folder);
            if (_historyList.TryGetValue(item, out var existingItem))
            {
                if (existingItem is not null)
                {
                    existingItem.Freshness = MaxFreshness;
                    existingItem.Folder.InvokeFreshnessUpdateEvent();
                }
            }
            else
            {
                if (_historyList.Count == MaxHistories)
                {
                    // Remove old item
                    _historyList.RemoveWhere((Item i) => i.Freshness <= 0);
                }

                item.Freshness = MaxFreshness;
                _historyList.Add(item);

                item.Folder.InvokeFreshnessUpdateEvent();
            }
        }

        //----------------------------------------------------------------------
        public int GetFreshness(Folder folder)
        {
            Item item = new(folder);
            if (_historyList.TryGetValue(item, out var existingItem))
            {
                return existingItem.Freshness;
            }

            return 0;
        }


        //======================================================================
        private class Item : IEquatable<Item>
        {
            public int Freshness { get; set; } = 0;
            public string Path { get; private set; }
            public Folder Folder { get; private set; }


            //------------------------------------------------------------------
            public Item(Folder folder)
            {
                Folder = folder;
                Path = folder.FetchFullPath();
                _hashCode = Path.GetHashCode();
            }

            //------------------------------------------------------------------
            public override int GetHashCode()
            {
                return _hashCode;
            }

            //------------------------------------------------------------------
            public bool Equals(Item? other)
            {
                if (GetHashCode() == other?.GetHashCode())
                {
                    return Path == other.Path;
                }

                return false;
            }


            private int _hashCode;
        }
    }
}
