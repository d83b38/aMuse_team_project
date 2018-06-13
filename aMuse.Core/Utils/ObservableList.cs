using System.Collections.ObjectModel;

namespace aMuse.Core.Utils
{
    public class ObservableList<T> : ObservableCollection<T>
    {
        private int _index = 0;

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

        public int GetIndex(T item)
        {
            return Items.IndexOf(item);
        }
    }
}
