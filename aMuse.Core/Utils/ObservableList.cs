using System;
using System.Collections.ObjectModel;

namespace aMuse.Core.Utils
{
    public class ObservableList<T> : ObservableCollection<T>
    {
        private int _index = 0;

        private static readonly Random rand = new Random();

        private void SetIndex(T item)
        {
            if (Items.Contains(item))
            {
                _index = Items.IndexOf(item);
            }
            else
            {
                _index = -1;
            }
        }

        /// <summary>
        /// Gets the next item in the collection
        /// </summary>
        /// <param name="item">item to get the following item for</param>
        /// <returns>the next item</returns>
        public T GetNext(T item)
        {
            SetIndex(item);

            if (_index != -1 && Items.Count > (_index + 1))
            {
                return Items[_index + 1];
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Gets the previous item in the collection
        /// </summary>
        /// <param name="item">item to get the previous item for</param>
        /// <returns>the previous item</returns>
        public T GetPrev(T item)
        {
            SetIndex(item);

            if (_index != -1 && _index > 0)
            {
                return Items[_index - 1];
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Gets index in the collection of the specified item
        /// </summary>
        /// <param name="item">the item to get index of</param>
        /// <returns>the index of the item in the collection</returns>
        public int GetIndex(T item)
        {
            return Items.IndexOf(item);
        }

        public T GetRandom()
        {
            return Items[rand.Next(0, Items.Count)];
        }
    }
}
