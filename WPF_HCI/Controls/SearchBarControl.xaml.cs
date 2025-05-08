using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF_HCI
{
    /// <summary>
    /// Code-behind for the SearchBarControl UserControl.
    /// This control allows querying emails by text input, selecting category, resetting the search,
    /// and choosing filters like "Unread" or "Important".
    /// 
    /// Requirements covered:
    /// - Custom events for search, reset, and filter (Requirement 2)
    /// - SearchBar delegates logic to parent, no filtering here (Requirement 2)
    /// - Placeholder management for usability
    /// </summary>
    public partial class SearchBarControl : UserControl
    {
        // (2.1) Custom event that notifies the parent when the search input or category changes
        public event EventHandler<SearchChangedEventArgs>? SearchChanged;

        // (2.2) Event triggered when the user clicks the "Reset" button
        public event EventHandler? SearchReset;

        // (2.3) Event triggered when the user changes the unread/important filter
        public event EventHandler<string>? FilterChanged;

        public SearchBarControl()
        {
            InitializeComponent(); // Load XAML structure
        }

        /// <summary>
        /// (Placeholder behavior) Clears placeholder text when textbox gains focus.
        /// </summary>
        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "Search...")
            {
                SearchBox.Text = "";
                SearchBox.Foreground = Brushes.White;
            }
        }

        /// <summary>
        /// (Placeholder behavior) Restores placeholder if textbox is left empty on losing focus.
        /// </summary>
        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SearchBox.Text = "Search...";
                SearchBox.Foreground = Brushes.White;
            }
        }

        /// <summary>
        /// (2.1) Raises SearchChanged whenever the search text changes (excluding placeholder).
        /// Passes the trimmed text and selected category (subject/sender/recipient).
        /// </summary>
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.Text == "Search...") return; // Ignore placeholder

            string category = (CategoryBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Subject";
            SearchChanged?.Invoke(this, new SearchChangedEventArgs(SearchBox.Text.Trim(), category));
        }

        /// <summary>
        /// (2.2) Clears input, resets category, and fires SearchReset event.
        /// </summary>
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "Search...";
            SearchBox.Foreground = Brushes.Gray;
            CategoryBox.SelectedIndex = 0; // Default to "Subject"
            SearchReset?.Invoke(this, EventArgs.Empty); // Notify parent
        }

        /// <summary>
        /// (2.3) Raises FilterChanged event with the selected filter ("All", "Unread", "Important").
        /// </summary>
        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterBox.SelectedItem is ComboBoxItem selected)
            {
                string option = selected.Content?.ToString() ?? "All";
                FilterChanged?.Invoke(this, option);
            }
        }
    }
}
