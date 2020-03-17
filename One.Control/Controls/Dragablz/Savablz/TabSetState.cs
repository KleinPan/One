using System.Collections.Generic;
using System.Linq;


namespace One.Control.Controls.Dragablz.Savablz
{
    public class TabSetState<TTabModel>
    {
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="selectedTabItemIndex">The index of the tab item that is currently selected in the TabSet</param>
        /// <param name="tabItems">The tab items</param>
        public TabSetState(int? selectedTabItemIndex, IEnumerable<TTabModel> tabItems)
        {
            this.SelectedTabItemIndex = selectedTabItemIndex;
            this.TabItems = tabItems.ToArray();
        }

        /// <summary>
        /// The tab item that is currently selected in the TabSet
        /// </summary>
        public int? SelectedTabItemIndex { get; }

        /// <summary>
        /// The tab items
        /// </summary>
        public TTabModel[] TabItems { get; }

    }
}
