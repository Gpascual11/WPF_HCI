using System;

namespace WPF_HCI
{
    /// <summary>
    /// Custom EventArgs used by the SearchBarControl to pass search information
    /// back to the parent (usually MainWindow).
    /// 
    /// This class carries:
    /// - The actual search query text entered by the user.
    /// - The selected search category (e.g., Subject, Sender, Recipient).
    /// 
    /// This satisfies Requirement 2 from the exercise:
    /// 2. "All user actions related to the user control should be forwarded to the rest of the
    /// application using dedicated events. You must define the type of event (and associated arguments) yourself."
    /// </summary>
    public class SearchChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the search term entered by the user.
        /// </summary>
        public string Query { get; }

        /// <summary>
        /// Gets the selected category for the search (e.g. Subject, Sender, Recipient).
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Constructor for SearchChangedEventArgs.
        /// Stores both the query string and selected category.
        /// </summary>
        /// <param name="query">The search text entered.</param>
        /// <param name="category">The category to filter by (e.g. Subject).</param>
        public SearchChangedEventArgs(string query, string category)
        {
            Query = query;
            Category = category;
        }
    }
}
