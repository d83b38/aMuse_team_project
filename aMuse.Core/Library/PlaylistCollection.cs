using System.Collections.ObjectModel;
using System.Linq;
using System;

namespace aMuse.Core.Library
{
    public class PlaylistCollection : ObservableCollection<Playlist>
    {
        public void RemoveWhere(Func<Playlist, bool> condition)
        {
            var itemsToRemove = this.Where(condition).ToList();

            foreach (var itemToRemove in itemsToRemove)
            {
                this.Remove(itemToRemove);
            }
        }
    }
}
