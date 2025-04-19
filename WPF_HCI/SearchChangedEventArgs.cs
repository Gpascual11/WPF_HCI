using System;

namespace WPF_HCI
{
    /// <summary>
    /// Custom EventArgs to carry search query and category from the search bar.
    /// </summary>
    public class SearchChangedEventArgs : EventArgs
    {
        public string Query { get; }
        public string Category { get; }

        public SearchChangedEventArgs(string query, string category)
        {
            Query = query;
            Category = category;
        }
    }
}
