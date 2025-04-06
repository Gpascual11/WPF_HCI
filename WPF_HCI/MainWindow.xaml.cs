using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_HCI
{
    public partial class MainWindow : Window
    {
        // Initialize the ViewModel inline so it is never null.
        public EmailViewModel ViewModel { get; set; } = new EmailViewModel();

        public MainWindow()
        {
            // Set the DataContext before InitializeComponent to ensure bindings can access the ViewModel.
            DataContext = ViewModel;
            InitializeComponent();
            this.WindowState = WindowState.Normal;
            ViewModel.LoadEmails();

            // Attach event handlers.
            FilterBox.SelectionChanged += FilterBox_SelectionChanged;
        }

        // Example event handler for the Exit menu item.
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void EmailList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (EmailList.SelectedItem is Email selectedEmail)
            {
                MessageBox.Show("Subject: " + selectedEmail.Subject,
                                "Email Subject",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "Search...")
            {
                SearchBox.Text = "";
                SearchBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SearchBox.Text = "Search...";
                SearchBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.Text == "Search...") return;
            ViewModel.FilterEmails(SearchBox.Text);
        }

        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ensure a non-null value by defaulting to "All" if null.
            string selectedFilter = ((ComboBoxItem?)FilterBox.SelectedItem)?.Content?.ToString() ?? "All";
            ViewModel.FilterByOption(selectedFilter);
        }


        private void ToggleImportant_Click(object sender, RoutedEventArgs e)
        {
            if (EmailList.SelectedItem is Email selectedEmail)
            {
                ViewModel.ToggleImportant(selectedEmail);
            }
        }

        private void FolderTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (FolderTree.SelectedItem is TreeViewItem selectedFolder)
            {
                string folderTag = selectedFolder.Tag?.ToString() ?? "";
                if (!string.IsNullOrEmpty(folderTag))
                {
                    ViewModel.FilterEmailsByFolder(folderTag);
                }
            }
        }

        // Optional empty handler if referenced in XAML.
        private void EmailList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // No additional logic needed.
        }

    }
}
