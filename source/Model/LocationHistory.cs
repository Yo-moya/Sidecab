
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


        private readonly HashSet<Item> _historyList = [];
        private int _maxFreshness = 10;
        private int _maxHistories = 10;


        //----------------------------------------------------------------------
        public void NotifyFolder(Folder folder)
        {
            // Age existing items
            foreach (var item in _historyList)
            {
                item.Freshness = Math.Max(0, item.Freshness - 1);
            }

            Item newItem = new(folder);

            if (_historyList.TryGetValue(newItem, out var existingItem)
                && (existingItem is not null))
            {
                existingItem.Freshness = MaxFreshness;
            }
            else
            {
                if (_historyList.Count == MaxHistories)
                {
                    // Remove old item
                    _historyList.RemoveWhere((Item i) => i.Freshness <= 0);
                }

                _historyList.Add(newItem);
                newItem.Freshness = MaxFreshness;
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
            //------------------------------------------------------------------
            public int Freshness
            {
                get => _freshness;
                set
                {
                    _freshness = value;
                    Folder.InvokeFreshnessUpdateEvent();
                }
            }

            public string Path { get; private set; }
            public Folder Folder { get; private set; }

            private readonly int _hashCode;
            private int _freshness = 0;


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
            public override bool Equals(object? obj)
            {
                return Equals(obj as Item);
            }

            //------------------------------------------------------------------
            public bool Equals(Item? other)
            {
                if ((other is not null) &&
                    (GetHashCode() == other.GetHashCode()))
                {
                    return Path == other.Path;
                }

                return false;
            }
        }
    }
}
