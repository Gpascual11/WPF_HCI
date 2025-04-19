using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF_HCI
{
    public partial class SearchBarControl : UserControl
    {
        // Custom event that sends query and category
        public event EventHandler<SearchChangedEventArgs>? SearchChanged;

        // Event for resetting the search
        public event EventHandler? SearchReset;

        // Event for changing the filter
        public event EventHandler<string>? FilterChanged;


        public SearchBarControl()
        {
            InitializeComponent();
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "Search...")
            {
                SearchBox.Text = "";
                SearchBox.Foreground = Brushes.Black;
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SearchBox.Text = "Search...";
                SearchBox.Foreground = Brushes.Gray;
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.Text == "Search...") return;

            string category = (CategoryBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Subject";
            SearchChanged?.Invoke(this, new SearchChangedEventArgs(SearchBox.Text.Trim(), category));
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "Search...";
            SearchBox.Foreground = Brushes.Gray;
            CategoryBox.SelectedIndex = 0;
            SearchReset?.Invoke(this, EventArgs.Empty);
        }

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
