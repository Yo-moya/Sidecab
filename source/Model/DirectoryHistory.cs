
using System;
using System.Collections.Generic;

namespace Sidecab.Model
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class DirectoryHistory
    {
        //----------------------------------------------------------------------
        public int MaxFreshness
        {
            get { return this.maxFreshness; }
            set { this.maxFreshness = Math.Max(1, value); }
        }

        //----------------------------------------------------------------------
        public int MaxHistories
        {
            get { return this.maxHistories; }
            set { this.maxHistories = Math.Max(1, value); }
        }


        //======================================================================
        public void NotifyDirectory(Directory directory)
        {
            // Age the history
            foreach (var i in this.historyList)
            {
                i.Freshness = Math.Max(0, i.Freshness - 1);
                i.Directory.InvokeFreshnessUpdateEvent();
            }

            Item existingItem;
            Item item = new Item(directory);

            //------------------------------------------------------------------
            if (this.historyList.TryGetValue(item, out existingItem))
            {
                existingItem.Freshness = this.MaxFreshness;
                existingItem.Directory.InvokeFreshnessUpdateEvent();
            }
            //------------------------------------------------------------------
            else
            {
                if (this.historyList.Count == this.MaxHistories)
                {
                    // Remove old item
                    this.historyList.RemoveWhere((Item i) => i.Freshness <= 0);
                }

                item.Freshness = this.MaxFreshness;
                this.historyList.Add(item);
                item.Directory.InvokeFreshnessUpdateEvent();
            }
            //------------------------------------------------------------------
        }

        //======================================================================
        public int GetFreshness(Directory directory)
        {
            Item existingItem;
            Item item = new Item(directory);

            if (this.historyList.TryGetValue(item, out existingItem))
            {
                return existingItem.Freshness;
            }

            return 0;
        }


        private HashSet<Item> historyList = new HashSet<Item>();
        private int maxFreshness = 10;
        private int maxHistories = 10;


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private class Item : IEquatable<Item>
        {
            public int Freshness { get; set; } = 0;
            public string Path { get; private set; }
            public Directory Directory { get; private set; }


            //==================================================================
            public Item(Directory directory)
            {
                this.Path = directory.Path;
                this.Directory = directory;
                this.hashCode = this.Path.GetHashCode();
            }

            //==================================================================
            public override int GetHashCode()
            {
                return this.hashCode;
            }

            //==================================================================
            public bool Equals(Item other)
            {
                if (this.GetHashCode() == other.GetHashCode())
                {
                    return this.Path == other.Path;
                }

                return false;
            }


            private int hashCode;
        }
    }
}
